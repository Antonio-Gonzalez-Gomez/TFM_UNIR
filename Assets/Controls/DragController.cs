using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DragController : MonoBehaviour,
    //Interfaces para la deteccion del raton
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] public bool isAugment = false;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    public event Action<HoverInfo> cardInfoShow;
    public event Action cardInfoHide;
    public event Action cardDragBegin;
    public event Action cardDragEnd;
    public event Action cardsSelected;

    private UITools uit;
    private Mouse mouse = Mouse.current;
    private DragCardSpot spot;
    private DragAugmentSpot augSpot;
    public bool selected = false;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = Quaternion.identity;
        uit = new UITools(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));
    }

    public void ResetPosition()
    {
        this.transform.SetPositionAndRotation(originalPosition, originalRotation);
    }
    public void SetSpot(DragCardSpot spot)
    {
        this.spot = spot;
    }
    public void SetSpot(DragAugmentSpot spot)
    {
        this.augSpot = spot;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (isDragging) { return; }

        if (isAugment)
        {
            //TODO: constructor HoverInfo con la info del aumento
        }
        else
        {
            cardInfoShow?.Invoke(new HoverInfo(this.GetComponentInParent<CardInstance>()));
        }
    }

    public void OnPointerExit(PointerEventData e)
    {
        cardInfoHide?.Invoke();
    }

    public void OnPointerClick(PointerEventData e)
    {
        //De momento, no se va a permitir seleccionar aumentos
        if (isAugment) { return; }

        if (selected)
        {
            spot.DeselectCard(this);
        }
        else
        {
            spot.TrySelectCard(this);
        }
        cardsSelected?.Invoke();
    }
    bool isDragging = false;
    public void OnBeginDrag(PointerEventData e)
    {
        //El pop up de informacion deberia ocultarse al arrastrar la carta
        cardInfoHide?.Invoke();
        cardDragBegin?.Invoke();
        isDragging = true;
    }

    void Update()
    {
        //Se hace asi porque OnDrag funcionaba regular
        if (isDragging)
        {
            Vector3 mousePosition = uit.CursorToWorldPosition(mouse.position.ReadValue());
            this.transform.SetPositionAndRotation(mousePosition, Quaternion.identity);
        }
    }

    //Para implementar IDragHandler, se mantiene este metodo (aunque no haga nada)
    //De lo contrario, OnBeginDrag/OnEndDrag no funcionan
    public void OnDrag(PointerEventData e)
    {

    }

    //TODO: ordenacion manual al soltar aumentos/cartas en el spot
    //Detectar hueco entre dos cartas/aumentos (al arrastrar carta) e insertar ahi
    public void OnEndDrag(PointerEventData e)
    {
        isDragging = false;
        if (isAugment)
        {
            AugmentEndDrag(e);
        }
        else
        {
            CardEngDrag(e);
        }
    }

    private void AugmentEndDrag(PointerEventData e)
    {
        ResetPosition();
        cardDragEnd?.Invoke();
    }

    private void CardEngDrag(PointerEventData e)
    {
        //Se comprueba que haya un spot en la posicion del raton al soltar la carta
        RaycastHit2D hit = Physics2D.Raycast(e.pointerCurrentRaycast.worldPosition, Vector2.zero, 0.001f, LayerMask.GetMask("CardSpot"));
        if (hit.collider != null)
        {
            CardInstance card = this.GetComponentInParent<CardInstance>();
            DragCardSpot newSpot = hit.collider.GetComponentInParent<DragCardSpot>();
            if (this.spot.allowInteract == true                     //Si el spot actual permite interacciones
                && newSpot != null                                  //Si el spot nuevo es de cartas
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
        this.selected = false;
        ResetPosition();
        cardDragEnd?.Invoke();
    }
}
