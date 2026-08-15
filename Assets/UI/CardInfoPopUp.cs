using TMPro;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class CardInfoPopUp : MonoBehaviour
{
    //TODO: tamaño de la ventana dinamico (en funcion del texto)
    [SerializeField] public TMP_Text cardTitle;
    [SerializeField] public TMP_Text cardDescription;

    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void ConnectEvents(DragController drag)
    {
        drag.cardInfoShow += OnCardShown;
        drag.cardInfoHide += OnCardHidden;
    }

    //TODO: Mover segun la posicion del raton?
    //Evento -> Añadir Vector3 con la posicion del popup
    //Dependiendo de donde este situada la carta (mano, cante, etc)
    //el popup deberia aparecer un poco hacia arriba/abajo/al lado de la posicion de la carta
    private void OnCardShown(CardInstance card)
    {
        //Aqui habra que meter la descripcion de los modificadores
        cardTitle.text = card.cartaBase.StringNombre();
        cardDescription.text = card.cartaBase.StringPuntos();
        canvas.enabled = true;
    }

    private void OnCardHidden()
    {
        canvas.enabled = false;
    }
}
