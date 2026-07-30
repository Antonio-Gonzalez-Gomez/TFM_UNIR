using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] CardInstance cardPrefab;
    [SerializeField] CardInfoPopUp cardInfoPopUp;

    //TODO: refactorizar la logica del DragController (posiciones) si la clase se vuelve muy grande
    [Header("Card Positions")]
    [SerializeField] Transform manoCentroPos;
    [SerializeField] float manoAncho;
    [SerializeField] Transform mazoRobarPos;
    [SerializeField] Transform cartaMuestraPos;
    [SerializeField] Transform cartaCroupierPos;
    [SerializeField] Transform cartaJugadaPos;
    [SerializeField] Transform canteCentroPos;
    [SerializeField] float canteAncho;

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

        //La carta de muestra se gira
        cartaMuestra = DrawCard(cartaMuestraPos.position, Quaternion.Euler(0f, 0f, 90f));
        cartaCroupier = DrawCard(cartaCroupierPos.position, Quaternion.identity);
        for (int i = 0; i < handSize; i++)
        {
            //La posicion es la del centro y el ancho la distancia entre el centro y uno de los bordes laterales
            //De esta forma, incX recorre el intervalo [-manoAncho, manoAncho] (asumiendo que el centro de la mano esta en x = 0)
            float incX = manoCentroPos.position.x - manoAncho + (2 * manoAncho * i) / handSize;
            //z = -i para que las cartas solapen bien en la mano
            Vector3 handCardPos = manoCentroPos.position + new Vector3(incX, 0f, -i);
            //TODO: rotacion cartas mano (abanico)
            //TODO: ordenaciones cartas (automaticas y manuales)
            CardInstance card = DrawCard(handCardPos, Quaternion.identity);
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
                    CardInstance card = Instantiate(cardPrefab, mazoRobarPos.position, Quaternion.identity);
                    card.InitCard(data);
                    mazoRobar.Add(card);
                }
            }
        }
    }

    //Roba una carta del mazo e inicializa su posicion y sprite
    private CardInstance DrawCard(Vector3 initialPosition, Quaternion initialRotation)
    {
        if (mazoRobar.Count == 0)
        {
            return null;
        }

        CardInstance res = mazoRobar[0];

        DragController drag = res.GetComponent<DragController>();
        drag.originalPosition = initialPosition;
        drag.originalRotation = initialRotation;
        //Los eventos usados para enseñar/ocultar informacion de la carta
        //Deben iniciarse desde el controlador de UI haciendo referencia al DragController
        cardInfoPopUp.ConnectDragEvents(drag);

        res.transform.position = initialPosition;
        res.transform.rotation = initialRotation;
        res.UpdateSprite();

        mazoRobar.RemoveAt(0);
        return res;
    }

}
