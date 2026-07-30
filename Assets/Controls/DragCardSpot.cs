using UnityEngine;
using System.Collections.Generic;

public class DragCardSpot : MonoBehaviour
{
    [SerializeField] public int maxCardAmount;
    [SerializeField] public float spotWidth;
    [SerializeField] bool allowInteract;
    [SerializeField] bool cardsInFan;
    [SerializeField] bool rotatedCard;

    public List<DragController> cardsInSpot;
    private float maxRotation = 10f;
    private float maxIncY = 0.2f;
    private void Awake()
    {
        cardsInSpot = new List<DragController>();
    }

    //TODO: logica reanclaje de cartas (mover de un spot a otro u ordenar dentro de la mano)
    //Permitir intercambiar posicion con otras cartas del spot si no hay hueco

    public void AddCard(DragController newDrag)
    {
        //Se comprueba que quede espacio
        if (cardsInSpot.Count > maxCardAmount)
            return;

        //Al haber varias cartas, se debe recalcular la posicion
        if (maxCardAmount > 1)
        {
            cardsInSpot.Add(newDrag);
            
            for (int i = 0; i < cardsInSpot.Count; i++)
            {
                DragController drag = cardsInSpot[i];

                //Partiendo de la posicion (centro del spot), se reparte la distancia a partir del numero de cartas
                //De esta forma, incX recorre el intervalo [position.x - spotWidth / 2, position.x + spotWidth / 2]
                float incX = this.transform.position.x - spotWidth / 2 + spotWidth * i / cardsInSpot.Count;
                //z = -i para que las cartas solapen bien en la mano
                drag.originalPosition = this.transform.position + new Vector3(incX, 0, -i);

                if (cardsInFan)
                {
                    //De manera similar, se calcula un ángulo de rotación que recorra el intervalo [-maxRotation, maxRotation]
                    float cardAngle = 2 * maxRotation * i / cardsInSpot.Count - maxRotation;
                    drag.originalRotation = Quaternion.Euler(0, 0, -cardAngle);
                    //Adicionalmente, se modifica ligeramente la posicion vertical de la carta para bajar ligeramente aquellas en los extremos
                    //El rango esta vez debería ser [incY, incY], por lo que se obtiene el valor absoluto
                    float incY = Mathf.Abs(2 * maxIncY * i / cardsInSpot.Count - maxIncY);
                    drag.originalPosition += new Vector3(0, -incY, 0);
                }

                drag.ResetPosition();
            }
        }

        else
        {
            //Si solo hay una carta en el sitio, la posicion es la del sitio
            newDrag.originalPosition = this.transform.position;
            newDrag.originalRotation = Quaternion.identity;
            //La carta de muestra aparece rotada
            if (rotatedCard)
                newDrag.originalRotation = Quaternion.Euler(0, 0, 90);

            newDrag.ResetPosition();
        }
    }
}
