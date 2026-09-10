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

    public event Action<HoverInfo> cardInfoShow;
    public event Action cardInfoHide;
    public event Action cardDragBegin;
    public event Action cardDragEnd;

    private DragCardSpot spot;
    public bool selected = false;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = Quaternion.identity;
    }

    public void ResetPosition()
    {
        this.transform.SetPositionAndRotation(originalPosition, originalRotation);
    }
    public void SetSpot(DragCardSpot spot)
    {
        this.spot = spot;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        cardInfoShow?.Invoke(new HoverInfo(this.GetComponentInParent<CardInstance>()));
    }

    public void OnPointerExit(PointerEventData e)
    {
        cardInfoHide?.Invoke();
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (selected)
        {
            spot.DeselectCard(this);
        }
        else
        {
            spot.TrySelectCard(this);
        }
    }
    public void OnBeginDrag(PointerEventData e)
    {
        //El pop up de informacion deberia ocultarse al arrastrar la carta
        //Evento para bloquear el cardInfo hasta que se suelte la carta?
        //TODO: evitar que los pop up de informacion de otras cartas aparezcan al hacer hover
        //En su lugar, deberia de activarse logica para desplazar las cartas
        cardInfoHide?.Invoke();
        cardDragBegin?.Invoke();
    }

    public void OnDrag(PointerEventData e)
    {
        //TODO: Si el cursor se mueve muy rapido, se puede cortar este evento aunque el click siga pulsado
        //Se debera de implementar mediante un input de click y un condicional (si el click se produjo al hacer hover)
        //Usar tweens!!!

        //Se cambia la posicion de Z para que al arrastrar una carta, se vea por encima de las demas
        Vector3 raton = e.pointerCurrentRaycast.worldPosition + new Vector3(0, 0, -50f);
        this.transform.SetPositionAndRotation(raton, Quaternion.identity);
    }

    public void OnEndDrag(PointerEventData e)
    {
        //Se comprueba que haya un spot en la posicion del raton al soltar la carta
        RaycastHit2D hit = Physics2D.Raycast(e.pointerCurrentRaycast.worldPosition, Vector2.zero, 0.001f, LayerMask.GetMask("CardSpot"));
        if (hit.collider != null)
        {
            CardInstance card = this.GetComponentInParent<CardInstance>();
            DragCardSpot newSpot = hit.collider.GetComponentInParent<DragCardSpot>();
            if (this.spot.allowInteract == true                     //Si el spot actual permite interacciones
                && newSpot.allowInteract == true                    //Si el spot nuevo permite interacciones
                && newSpot.cardList.Count < newSpot.maxCardAmount   //Si tiene hueco para otra carta
                && !newSpot.cardList.Contains(card))                //Si esta carta no estaba ya en el spot
            {
                //Se elimina la carta de su anterior spot
                this.spot.RemoveCard(card);
                //Y se añade al nuevo (donde haya apuntado el raton)
                newSpot.AddCard(card);
                this.spot = newSpot;
            }
        }
        ResetPosition();
        cardDragEnd?.Invoke();
    }
}
