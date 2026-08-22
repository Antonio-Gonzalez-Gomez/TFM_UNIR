using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] CardInstance cardPrefab;
    [SerializeField] CardInfoPopUp cardInfoPopUp;
    [SerializeField] PlayButtonsController playButtons;

    [Header("Card Spots")]
    [SerializeField] DragCardSpot manoSpot;
    [SerializeField] DragCardSpot mazoRobarSpot;
    [SerializeField] DragCardSpot muestraSpot;
    [SerializeField] DragCardSpot rivalSpot;
    [SerializeField] DragCardSpot canteSpot;
    [SerializeField] DragCardSpot bazaSpot;

    //Este valor puede que sea dinamico en un futuro por aumentos u otros efectos
    private int handSize = 8;

    void Start()
    {
        List<CardInstance> cartasMazo = InitDeck();
        DeckShuffler.Shuffle(cartasMazo);
        mazoRobarSpot.cardList = cartasMazo;

        DrawCard(muestraSpot);
        DrawCard(rivalSpot);
        for (int i = 0; i < handSize; i++)
        {
            DrawCard(manoSpot);
        }
    }

    //Funcion para gestionar los drag controller (posicion inicial, rotacion)

    private List<CardInstance> InitDeck()
    {
        List<CardInstance> res = new List<CardInstance>();
        //Se genera una baraja española (40 cartas, 10 de cada palo, 4 de cada valor)
        //Iterando sobre los enum definidos
        foreach (Palo palo in (Palo[]) Enum.GetValues(typeof(Palo)))
        {
            foreach (Valor valor in (Valor[])Enum.GetValues(typeof(Valor)))
            {
                //Las puntuaciones de las cartas se han hardcodeado en una clase aparte por mayor comodidad
                //No son valores que sea previsible que se quieran modificar en un futuro
                if(PuntuacionesCartas.cardPoints.TryGetValue(valor, out int puntos))
                {
                    //No se puede crear directamente un SO, hay que actualizar los valores a parte
                    CardData data = ScriptableObject.CreateInstance<CardData>();
                    data.Valor = valor;
                    data.Palo = palo;
                    data.Puntos = puntos;
                    //El CardInstance debe instanciarse en escena (aunque permanezca en el mazo)
                    CardInstance card = Instantiate(cardPrefab, mazoRobarSpot.transform.position, Quaternion.identity);
                    card.InitCard(data);
                    res.Add(card);
                }
            }
        }

        return res;
    }

    //Roba una carta del mazo e inicializa su sprite y componente de DragController
    private CardInstance DrawCard(DragCardSpot spot)
    {
        //Comprobación de cartas en el mazo
        if (mazoRobarSpot.cardList.Count == 0)
        {
            return null;
        }

        CardInstance res = mazoRobarSpot.cardList[0];
        DragController drag = res.GetComponent<DragController>();
        spot.AddCard(res);
        //Los eventos usados para enseñar/ocultar informacion de la carta
        //Deben iniciarse desde el controlador de UI haciendo referencia al DragController
        cardInfoPopUp.ConnectEvents(drag);
        //Lo mismo para los eventos de los botones
        playButtons.ConnectEvents(drag);
        res.UpdateSprite();

        mazoRobarSpot.cardList.RemoveAt(0);
        return res;
    }

    //Mueve cartas desde la mano hasta el hueco para las cartas de cante o la de baza
    public void MoveSelectedCardsToSpot(bool isCanteSpot)
    {
        DragCardSpot spot = isCanteSpot ? canteSpot : bazaSpot;
        List<CardInstance> selectedCards = manoSpot.GetSelectedCards();

        //Si hay mas cartas seleccionadas de las permitidas en el hueco
        if (selectedCards.Count > spot.maxCardAmount)
        {
            return;
        }

        //Si habia cartas en el hueco y no cogen las nuevas, se quitan las viejas
        if (spot.cardList.Count > 0 && selectedCards.Count + spot.cardList.Count > spot.maxCardAmount)
        {
            spot.cardList.ForEach(x => manoSpot.AddCard(x));
            spot.ClearSpot();
        }
        //Se mueven las cartas de la mano al hueco
        foreach (CardInstance selCard in selectedCards)
        {
            manoSpot.RemoveCard(selCard);
            spot.AddCard(selCard);
        }
    }
}
