using UnityEngine;

public class CardInstance : MonoBehaviour
{
    public CardData cartaBase;
    //aqui tambien irian los modificadores
    private SpriteRenderer spriteRenderer;

    public CardInstance(CardData cartaBase)
    {
        this.cartaBase = cartaBase;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Al convertir los datos de la carta a int, se obtiene la ordenacion del enum
        //Oros -> 0, Copas -> 1, As -> 0, Sota -> 7, etc
        int spriteIndex = (int)cartaBase.palo * 12 + (int)cartaBase.valor;
        spriteRenderer.sprite = SpriteSelector.GetSpriteByIndex(spriteIndex);
    }

}
