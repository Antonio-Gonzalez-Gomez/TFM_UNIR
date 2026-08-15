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
    public event Action playButtonsShow;
    public event Action playButtonsHide;

    //TODO: refactorizar: quitar esta referencia y obtenerla dinamicamente en el evento
    public CardInstance card;
    private DragCardSpot spot;
    public bool selected = false;

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
    public void SetSpot(DragCardSpot spot)
    {
        this.spot = spot;
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
        //TODO: evitar que los pop up de informacion de otras cartas aparezcan al hacer hover
        //En su lugar, deberia de activarse logica para desplazar las cartas
        waitingInfo = false;
        cardInfoHide?.Invoke();
        playButtonsHide?.Invoke();
    }

    public void OnDrag(PointerEventData e)
    {
        //TODO: Si el cursor se mueve muy rapido, se puede cortar este evento aunque el click siga pulsado
        //Se debera de implementar mediante un input de click y un condicional (si el click se produjo al hacer hover)
        this.transform.SetPositionAndRotation(e.pointerCurrentRaycast.worldPosition, Quaternion.identity);
    }

    public void OnEndDrag(PointerEventData e)
    {
        //Se comprueba que haya un spot en la posicion del raton al soltar la carta
        RaycastHit2D hit = Physics2D.Raycast(e.pointerCurrentRaycast.worldPosition, Vector2.zero, 0.001f, LayerMask.GetMask("CardSpot"));
        if (hit.collider != null)
        {
            DragCardSpot newSpot = hit.collider.GetComponentInParent<DragCardSpot>();
            if (newSpot.allowInteract == true  //Si el spot permite interacciones
                && newSpot.cardsInSpot.Count < newSpot.maxCardAmount  //Si tiene hueco para otra carta
                && !newSpot.cardsInSpot.Contains(this))    //Si esta carta no estaba ya en el spot
            {
                //Se elimina la carta de su anterior spot
                this.spot.RemoveCard(this);
                //Y se añade al nuevo (donde haya apuntado el raton)
                newSpot.AddCard(this);
                this.spot = newSpot;
            }
        }
        ResetPosition();
        playButtonsShow?.Invoke();
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
