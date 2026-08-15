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

    public List<DragController> cardsInSpot;
    private int maxSelectedCards = 4;
    private float maxRotation = 10f;
    private float maxIncY = 0.2f;
    private float cardIncX = 1f;
    private void Awake()
    {
        cardsInSpot = new List<DragController>();
    }

    //TODO: Reordenacion cartas
    //Permitir intercambiar posicion con otras cartas del spot si no hay hueco?

    private void UpdateCardsPositions()
    {
        for (int i = 0; i < cardsInSpot.Count; i++)
        {
            DragController drag = cardsInSpot[i];

            //Partiendo de la posicion (centro del spot), se reparte la distancia a partir del numero de cartas
            //De esta forma, incX recorre el intervalo [position.x - spotWidth / 2, position.x + spotWidth / 2]
            //Se le añade la mitad del ancho de una carta para centrar las posiciones (y que no se apelotonen en la izquierda)
            float incX = this.transform.position.x - spotWidth / 2 + spotWidth * i / cardsInSpot.Count + cardIncX;
            //z = -i para que las cartas solapen bien en la mano
            drag.originalPosition = this.transform.position + new Vector3(incX, 0, -i);
            drag.originalRotation = Quaternion.identity;

            if (cardsInFan)
            {
                //De manera similar, se calcula un ángulo de rotación que recorra el intervalo [-maxRotation, maxRotation]
                float cardAngle = 2 * maxRotation * i / cardsInSpot.Count - maxRotation;
                drag.originalRotation = Quaternion.Euler(0, 0, -cardAngle);
                //Adicionalmente, se modifica ligeramente la posicion vertical de la carta para bajar ligeramente aquellas en los extremos
                //El rango esta vez debería ser [incY, incY], por lo que se obtiene el valor absoluto
                float incY = Mathf.Abs(2 * maxIncY * i / cardsInSpot.Count - maxIncY);
                drag.originalPosition += new Vector3(0f, -incY, 0); //TODO: en las cartas de la mano se apelotonan a la izquierda??
            }
            drag.selected = false;
            drag.ResetPosition();
        }
    }

    //Metodo que intenta enlazar un DragController con este DragCardSpot
    //Actualiza ademas sus parametros de posicion/rotacion
    public void AddCard(DragController newDrag)
    {
        //Se comprueba que quede espacio
        if (cardsInSpot.Count >= maxCardAmount)
            return;

        cardsInSpot.Add(newDrag);
        newDrag.SetSpot(this);
        //Al haber varias cartas, se debe recalcular la posicion de todas
        if (maxCardAmount > 1)
        {
            UpdateCardsPositions();
        }

        else
        {
            //Si solo hay una carta en el sitio, la posicion es la del sitio (centrada)
            newDrag.originalPosition = this.transform.position;
            newDrag.originalRotation = Quaternion.identity;
            //La carta de muestra aparece rotada
            if (rotatedCard)
                newDrag.originalRotation = Quaternion.Euler(0, 0, 90);

            newDrag.ResetPosition();
        }
    }

    public void RemoveCard(DragController removedDrag)
    {
        cardsInSpot.Remove(removedDrag);
        removedDrag.selected = false;
        //Actualizar posiciones del resto de cartas
        if (maxCardAmount > 1)
        {
            UpdateCardsPositions();
        }
    }

    public List<CardInstance> ClearSpot()
    {
        List<CardInstance> res = new List<CardInstance>();
        cardsInSpot.ForEach(x => res.Add(x.card));

        cardsInSpot.Clear();
        return res;
    }

    public List<DragController> GetSelectedCards()
    {
        return cardsInSpot.FindAll(x => x.selected);
    }
    public void TrySelectCard(DragController drag)
    {
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
