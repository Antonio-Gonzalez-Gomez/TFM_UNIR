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

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = Quaternion.identity;
    }
    public void OnPointerEnter(PointerEventData e)
    {
        //Mostrar info al cabo de un delay
    }

    public void OnPointerExit(PointerEventData e)
    {

    }

    public void OnPointerClick(PointerEventData e)
    {
        //Seleccionar
    }
    public void OnBeginDrag(PointerEventData e)
    {
    
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
}
