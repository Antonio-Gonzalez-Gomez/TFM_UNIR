using UnityEngine;
using System.Collections.Generic;

public class DragCardSpot : MonoBehaviour
{
    [SerializeField] public int maxCardAmount;
    [SerializeField] public float spotWidth;
    [SerializeField] public bool allowInteract;
    [SerializeField] bool cardsInFan;
    [SerializeField] bool rotatedCard;
    [SerializeField] bool allowSelection;

    public List<CardInstance> cardList;
    private int maxSelectedCards = 4;

    private float maxRotation = 10f;
    private float maxIncY = 0.2f;
    private float cardIncX = 1f;
    private void Awake()
    {
        cardList = new List<CardInstance>();
    }

    //TODO: Reordenacion cartas
    //Permitir intercambiar posicion con otras cartas del spot si no hay hueco?

    private void UpdateCardsPositions()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            DragController drag = cardList[i].drag;

            //Partiendo de la posicion (centro del spot), se reparte la distancia a partir del numero de cartas
            //De esta forma, incX recorre el intervalo [position.x - spotWidth / 2, position.x + spotWidth / 2]
            //Se le añade la mitad del ancho de una carta para centrar las posiciones (y que no se apelotonen en la izquierda)
            float incX = this.transform.position.x - spotWidth / 2 + spotWidth * i / cardList.Count + cardIncX;
            //z = -i para que las cartas solapen bien en la mano
            drag.originalPosition = this.transform.position + new Vector3(incX, 0, -i);
            drag.originalRotation = Quaternion.identity;

            if (cardsInFan)
            {
                //De manera similar, se calcula un ángulo de rotación que recorra el intervalo [-maxRotation, maxRotation]
                float cardAngle = 2 * maxRotation * i / cardList.Count - maxRotation;
                drag.originalRotation = Quaternion.Euler(0, 0, -cardAngle);
                //Adicionalmente, se modifica ligeramente la posicion vertical de la carta para bajar ligeramente aquellas en los extremos
                //El rango esta vez debería ser [incY, incY], por lo que se obtiene el valor absoluto
                float incY = Mathf.Abs(2 * maxIncY * i / cardList.Count - maxIncY);
                drag.originalPosition += new Vector3(0f, -incY, 0); //TODO: en las cartas de la mano se apelotonan a la izquierda??
            }
            drag.selected = false;
            drag.ResetPosition();
        }
    }

    //Metodo que intenta enlazar un DragController con este DragCardSpot
    //Actualiza ademas sus parametros de posicion/rotacion
    public void AddCard(CardInstance newCard)
    {
        DragController drag = newCard.drag;
        //Se comprueba que quede espacio
        if (cardList.Count >= maxCardAmount)
            return;

        cardList.Add(newCard);
        drag.SetSpot(this);
        //Al haber varias cartas, se debe recalcular la posicion de todas
        if (maxCardAmount > 1)
        {
            UpdateCardsPositions();
        }

        else
        {
            //Si solo hay una carta en el sitio, la posicion es la del sitio (centrada)
            drag.originalPosition = this.transform.position;
            drag.originalRotation = Quaternion.identity;
            //La carta de muestra aparece rotada
            if (rotatedCard)
                drag.originalRotation = Quaternion.Euler(0, 0, 90);

            drag.ResetPosition();
        }
    }

    public void RemoveCard(CardInstance removedCard)
    {
        cardList.Remove(removedCard);
        removedCard.drag.selected = false;
        //Actualizar posiciones del resto de cartas
        if (maxCardAmount > 1)
        {
            UpdateCardsPositions();
        }
    }

    public List<CardInstance> ClearSpot()
    {
        //Copia de la lista a borrar
        List<CardInstance> res = new List<CardInstance>(cardList);
        cardList.Clear();
        return res;
    }

    public List<CardInstance> GetSelectedCards()
    {
        return cardList.FindAll(x => x.drag.selected);
    }
    public void TrySelectCard(DragController drag)
    {
        //Si el hueco permite seleccionar cartas y el maximo de cartas seleccionadas no se ha alcanzado
        if (allowSelection && maxSelectedCards > GetSelectedCards().Count)
        {
            drag.selected = true;
            drag.transform.position = drag.originalPosition + new Vector3(0f, 0.25f, 0f);
        }
    }

    public void DeselectCard(DragController drag)
    {
        drag.selected = false;
        drag.transform.position = drag.originalPosition;
    }
}
