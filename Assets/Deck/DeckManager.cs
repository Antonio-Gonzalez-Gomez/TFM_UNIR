using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] CardInstance cardPrefab;
    [SerializeField] InfoPopUp cardInfoPopUp;
    [SerializeField] ScoreUIController uiController;

    [Header("Card Spots")]
    [SerializeField] DragCardSpot manoSpot;
    [SerializeField] DragCardSpot mazoRobarSpot;
    [SerializeField] DragCardSpot muestraSpot;
    [SerializeField] DragCardSpot rivalSpot;
    [SerializeField] DragCardSpot canteSpot;
    [SerializeField] DragCardSpot bazaSpot;

    //Los descartes no necesitan la lógica de DragCardSpot
    private List<CardInstance> descartes;

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

        descartes = new List<CardInstance>();
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



    //Método que roba una carta al azar y la añade al DragCardSpot
    public CardInstance DrawCard(DragCardSpot spot)
    {
        //Comprobación de cartas en el mazo
        if (mazoRobarSpot.cardList.Count == 0)
        {
            return null;
        }

        CardInstance res = mazoRobarSpot.cardList[0];
        InitDrawnCard(spot, res);

        //TEMPORAL
        //AddRandomMods(res);

        return res;
    }

    //Método que inicializa los eventos y DragController de una carta
    public void InitDrawnCard(DragCardSpot spot, CardInstance card)
    {
        DragController drag = card.GetComponent<DragController>();
        spot.AddCard(card);
        //Los eventos usados para enseñar/ocultar informacion de la carta
        //Deben iniciarse desde el controlador de UI haciendo referencia al DragController
        cardInfoPopUp.ConnectEvents(drag);
        //Lo mismo para los eventos de los botones
        uiController.ConnectEvents(drag);
        card.UpdateSprite();

        mazoRobarSpot.cardList.RemoveAt(0);
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

    public void PrepareNextHand()
    {
        //Descarta las cartas utilizadas
        DiscardCardsInSpot(bazaSpot);
        DiscardCardsInSpot(canteSpot);
        DiscardCardsInSpot(rivalSpot);

        //Roba cartas nuevas
        DrawCard(rivalSpot);
        int handCardsMissing = handSize - manoSpot.cardList.Count;
        for (int i = 0; i < handCardsMissing; i++)
        {
            DrawCard(manoSpot);
        }
    }

    //Descarta todas las cartas de un spot y las manda a la pila de descartes
    private void DiscardCardsInSpot(DragCardSpot originalSpot)
    {
        List<CardInstance> discarded = originalSpot.ClearSpot();
        //Saca las cartas de la pantalla
        discarded.ForEach(x => x.transform.position = new Vector3(-100f, -100f, 0));
        descartes.AddRange(discarded);
    }

    //TEMPORAL PARA PROBAR MODS
    private void AddRandomMods(CardInstance card)
    {
        int alpha = UnityEngine.Random.Range(0, 10);
        int beta = UnityEngine.Random.Range(0, 10);

        switch (alpha)
        {
            case 0:
                card.AddModifier(new M_Strength());
                break;
            case 1:
                card.AddModifier(new M_Dexterity());
                break;
            case 2:
                card.AddModifier(new M_Plague());
                break;
            case 3:
                card.AddModifier(new M_Fortune());
                break;
            case 4:
                card.AddModifier(new M_Coinflip());
                break;
            case 5:
                card.AddModifier(new M_Solitude());
                break;
            default:
                break;
        }

        switch (beta)
        {
            case 0:
                card.AddModifier(new M_Hunter());
                break;
            case 1:
                card.AddModifier(new M_Bastion());
                break;
            case 2:
                card.AddModifier(new M_Polivalence());
                break;
            case 3:
                card.AddModifier(new M_Recycling());
                break;
            case 4:
                card.AddModifier(new M_Clone());
                break;
            case 5:
                card.AddModifier(new M_Summon());
                break;
            default:
                break;
        }

        switch(card.cartaBase.Valor)
        {
            case Valor.As:
            case Valor.Tres:
                card.AddModifier(new M_Alchemy(card.puntos));
                break;

            case Valor.Dos:
            case Valor.Cuatro:
                card.AddModifier(new M_Counter());
                break;

            case Valor.Cinco:
                card.AddModifier(new M_Mirror(card.cartaBase));
                break;

            case Valor.Seis:
                card.AddModifier(new M_Switch());
                break;

            case Valor.Siete:
                card.AddModifier(new M_Relief());
                break;

            default:
                card.AddModifier(new M_Encore());
                break;
        }
    }
}
