using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] CardInstance cardPrefab;
    [SerializeField] CardInfoPopUp cardInfoPopUp;

    [Header("Card Spots")]
    [SerializeField] DragCardSpot cartasManoPos;
    [SerializeField] DragCardSpot mazoRobarPos;
    [SerializeField] DragCardSpot cartaMuestraPos;
    [SerializeField] DragCardSpot cartaCroupierPos;
    [SerializeField] DragCardSpot cartasCantePos;
    [SerializeField] DragCardSpot cartaJugadaPos;

    private List<CardInstance> mazoRobar;
    private List<CardInstance> mazoDescartes;
    private List<CardInstance> cartasMano;
    private CardInstance cartaMuestra;
    private CardInstance cartaCroupier;
    private CardInstance cartaJugada;
    private List<CardInstance> cartasCante;
    //Este valor puede que sea dinamico en un futuro por aumentos u otros efectos
    private int handSize = 8;

    void Start()
    {
        InitDeck();
        DeckShuffler.Shuffle(mazoRobar);

        cartaMuestra = DrawCard(cartaMuestraPos);
        cartaCroupier = DrawCard(cartaCroupierPos);
        for (int i = 0; i < handSize; i++)
        {
            CardInstance card = DrawCard(cartasManoPos);
            cartasMano.Add(card);
        }
    }

    //Funcion para gestionar los drag controller (posicion inicial, rotacion)

    private void InitDeck()
    {
        mazoRobar = new List<CardInstance>();
        mazoDescartes = new List<CardInstance>();
        cartasMano = new List<CardInstance>();
        cartasCante = new List<CardInstance>();

        //Se genera una baraja española (40 cartas, 10 de cada palo, 4 de cada valor)
        //Iterando sobre los enum definidos
        foreach (Palo palo in (Palo[]) Enum.GetValues(typeof(Palo)))
        {
            foreach (Valor valor in (Valor[])Enum.GetValues(typeof(Valor)))
            {
                //Las puntuaciones de las cartas se han hardcodeado en una clase aparte por mayor comodidad
                //No son valores que sea previsible que se quieran modificar en un futuro
                if(PuntuacionesCartas.dict.TryGetValue(valor, out int puntos))
                {
                    //No se puede crear directamente un SO, hay que actualizar los valores a parte
                    CardData data = ScriptableObject.CreateInstance<CardData>();
                    data.Valor = valor;
                    data.Palo = palo;
                    data.Puntos = puntos;
                    //El CardInstance debe instanciarse en escena (aunque permanezca en el mazo)
                    CardInstance card = Instantiate(cardPrefab, mazoRobarPos.transform.position, Quaternion.identity);
                    card.InitCard(data);
                    mazoRobar.Add(card);
                }
            }
        }
    }

    //Roba una carta del mazo e inicializa su sprite y componente de DragController
    private CardInstance DrawCard(DragCardSpot spot)
    {
        //Comprobación de cartas en el mazo
        if (mazoRobar.Count == 0)
        {
            return null;
        }

        CardInstance res = mazoRobar[0];
        DragController drag = res.GetComponent<DragController>();
        spot.AddCard(drag);
        //Los eventos usados para enseñar/ocultar informacion de la carta
        //Deben iniciarse desde el controlador de UI haciendo referencia al DragController
        cardInfoPopUp.ConnectDragEvents(drag);
        res.UpdateSprite();

        mazoRobar.RemoveAt(0);
        return res;
    }

}
