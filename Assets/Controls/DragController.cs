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


    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = Quaternion.identity;

        card = this.GetComponentInParent<CardInstance>();
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
        waitingInfo = false;
        cardInfoHide?.Invoke();
    }

    public void OnPointerClick(PointerEventData e)
    {
        //Seleccionar
    }
    public void OnBeginDrag(PointerEventData e)
    {
        //El pop up de informacion deberia ocultarse al arrastrar la carta
        cardInfoHide?.Invoke();
    }

    public void OnDrag(PointerEventData e)
    {
        //TODO: Si el cursor se mueve muy rapido, se puede cortar este evento aunque el click siga pulsado
        //Se debera de implementar mediante un input de click y un condicional (si el click se produjo al hacer hover)
        this.transform.position = e.pointerCurrentRaycast.worldPosition;
        this.transform.rotation = Quaternion.identity;
    }

    public void OnEndDrag(PointerEventData e)
    {
        this.transform.position = originalPosition;
        this.transform.rotation = originalRotation;
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
