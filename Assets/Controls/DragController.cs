using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour,
    //Interfaces para la deteccion del raton
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    public event Action<CardInstance> cardInfoShow;
    public event Action cardInfoHide;

    private CardInstance card;
    private bool selected = false;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = Quaternion.identity;

        card = this.GetComponentInParent<CardInstance>();
    }

    public void ResetPosition()
    {
        this.transform.SetPositionAndRotation(originalPosition, originalRotation);
    }

    private float infoTimer = 0f;
    private bool waitingInfo = false;
    public void OnPointerEnter(PointerEventData e)
    {
        waitingInfo = true;
        infoTimer = 0.5f;
    }

    public void OnPointerExit(PointerEventData e)
    {
        //TODO: si se pasa a hacer hover de otra carta, saltarse el tiempo de espera y enseñar inmediatamente
        waitingInfo = false;
        cardInfoHide?.Invoke();
    }

    public void OnPointerClick(PointerEventData e)
    {
        waitingInfo = false;
        selected = !selected;
        if (selected)
        {
            this.transform.position += new Vector3(0f, 0.25f, 0f);
        }
        else
        {
            this.transform.position = originalPosition;
        }
    }
    public void OnBeginDrag(PointerEventData e)
    {
        //El pop up de informacion deberia ocultarse al arrastrar la carta
        //TODO: evitar que los pop up de informacion de otras cartas aparezcan al hacer hover
        //En su lugar, deberia de activarse logica para desplazar las cartas
        waitingInfo = false;
        cardInfoHide?.Invoke();
    }

    public void OnDrag(PointerEventData e)
    {
        //TODO: Si el cursor se mueve muy rapido, se puede cortar este evento aunque el click siga pulsado
        //Se debera de implementar mediante un input de click y un condicional (si el click se produjo al hacer hover)
        this.transform.SetPositionAndRotation(e.pointerCurrentRaycast.worldPosition, Quaternion.identity);
    }

    public void OnEndDrag(PointerEventData e)
    {
        ResetPosition();
    }

    private void Update()
    {
        if (waitingInfo)
        {
            infoTimer -= Time.deltaTime;
            if (infoTimer < 0f)
            {
                cardInfoShow?.Invoke(card);
                waitingInfo = false;
            }
        }
    }
}
