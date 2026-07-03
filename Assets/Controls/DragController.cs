using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{

    public void OnPointerEnter(PointerEventData e)
    {
        Debug.Log("Card hover");
    }

    public void OnPointerExit(PointerEventData e)
    {

    }

    public void OnPointerClick(PointerEventData e)
    {

    }
    public void OnBeginDrag(PointerEventData e)
    {
    
    }

    public void OnDrag(PointerEventData e)
    {
        //Si el cursor se mueve muy rapido, se puede terminar este evento aunque el click siga pulsado
        //Se debera de implementar mediante un input de click y un condicional (si el click se produjo al hacer hover) de cara a futuro
        this.transform.position = e.pointerCurrentRaycast.worldPosition;
    }

    public void OnEndDrag(PointerEventData e)
    {
    
    }
}
