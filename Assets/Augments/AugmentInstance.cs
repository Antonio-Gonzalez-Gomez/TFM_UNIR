using TMPro;
using UnityEngine;

public class AugmentInstance : MonoBehaviour
{
    public AugmentData data;
    public DragController drag;
    private TMP_Text tmp;
    //TODO: subida de tier
    //En teoria, deberia de dejarse en la seccion de data los mecanismos para cuantificar la xp
    //Pero esta clase deberia tener un metodo que "cambie" de un tier a otro (aka cambiar el atributo aug)

    private void Awake()
    {
        drag = GetComponent<DragController>();
        tmp = GetComponentInChildren<TMP_Text>();
    }

    public void SetAugment(AugmentData data)
    {
        this.data = data;
        tmp.text = data.Name;
        data.ParentReference = this;
    }
}
