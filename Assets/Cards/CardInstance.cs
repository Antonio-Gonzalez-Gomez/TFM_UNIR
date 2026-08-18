using UnityEngine;

public class CardInstance : MonoBehaviour
{
    public CardData cartaBase;
    public Modifier alphaMod;
    public Modifier betaMod;
    public Modifier gammaMod;

    private SpriteRenderer spriteRenderer;
    public DragController drag;
    //Indice del sprite con el reverso de las cartas
    private const int reversoIndex = 41;

    public CardInstance(CardData cartaBase)
    {
        this.cartaBase = cartaBase;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        drag = GetComponent<DragController>();
    }

    public void InitCard(CardData cartaBase)
    {
        this.cartaBase = cartaBase;
        //Le da la "vuelta" a la carta al quedar inicializada en el mazo de robo
        //Al robar la carta, se llama a UpdateSprite para obtener el sprite de verdad
        spriteRenderer.sprite = SpriteSelector.GetSpriteByIndex(reversoIndex);
    }

    public void UpdateSprite()
    {
        //Al convertir los datos de la carta a int, se obtiene la ordenacion del enum
        //Oros -> 0, Copas -> 1, As -> 0, Sota -> 7, etc
        int spriteIndex = (int)cartaBase.Palo * 10 + (int)cartaBase.Valor;
        spriteRenderer.sprite = SpriteSelector.GetSpriteByIndex(spriteIndex);
    }
    public bool EsCartaFigura()
    {
        return cartaBase.Valor == Valor.Sota || cartaBase.Valor == Valor.Caballo || cartaBase.Valor == Valor.Rey;
    }

    public void ScoreCard(ScoreManager sm)
    {
        sm.puntosJugada += cartaBase.Puntos;
        sm.InvokePointScore(cartaBase.Puntos);

        if (alphaMod != null)
            alphaMod.OnCardScore(sm);
        if (betaMod != null)
            betaMod.OnCardScore(sm);
        if (gammaMod != null)
            gammaMod.OnCardScore(sm);
    }
}
