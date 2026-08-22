using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CardInstance : MonoBehaviour
{
    public CardData cartaBase;

    private List<Modifier> modifiers;
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

    //Inicializa la carta con el sprite del reverso (para que quede sobre el mazo)
    public void InitCard(CardData cartaBase)
    {
        this.cartaBase = cartaBase;
        //Le da la "vuelta" a la carta al quedar inicializada en el mazo de robo
        //Al robar la carta, se llama a UpdateSprite para obtener el sprite de verdad
        spriteRenderer.sprite = SpriteSelector.GetSpriteByIndex(reversoIndex);
    }

    //Actualiza la carta al sprite correspondiente
    public void UpdateSprite()
    {
        //Al convertir los datos de la carta a int, se obtiene la ordenacion del enum
        //Oros -> 0, Copas -> 1, As -> 0, Sota -> 7, etc
        int spriteIndex = (int)cartaBase.Palo * 10 + (int)cartaBase.Valor;
        spriteRenderer.sprite = SpriteSelector.GetSpriteByIndex(spriteIndex);
    }

    public Modifier GetMod(ModifierType type)
    {
        return modifiers.Find(x => x.Type == type);
    }

    //Añade un modificador, sustituyendo el anterior del mismo tipo
    public void AddModifier(Modifier mod, ModifierType type)
    {
        Modifier alphaMod = type == ModifierType.Alpha ? mod : GetMod(ModifierType.Alpha);
        Modifier betaMod = type == ModifierType.Beta ? mod : GetMod(ModifierType.Beta);
        Modifier gammaMod = type == ModifierType.Gamma ? mod : GetMod(ModifierType.Gamma);

        //Es necesario rehacer la lista de modificadores para que guarden el orden correcto
        modifiers = new List<Modifier>();
        if (alphaMod != null)
            modifiers.Add(alphaMod);
        if (betaMod != null)
            modifiers.Add(betaMod);
        if (gammaMod != null)
            modifiers.Add(gammaMod);
    }

    public int CompararPalo(CardInstance otraCarta)
    {
        int result = -1;

        //Solo existe un modificador que altere los palos

        return result;
    }

    //Método base que compara y devuelve un entero en función del valor de ambas
    //0: el valor de ambas es equivalente
    //1: esta carta es superior
    //2: la otra carta es superior
    public int CompararValor(Valor otroValor)
    {
        //Este diccionario ordena las cartas en funcion de su poder (As = 1, 3 = 2, Rey = 3, etc)
        PuntuacionesCartas.cardWinOrder.TryGetValue(this.cartaBase.Valor, out int playerOrder);
        PuntuacionesCartas.cardWinOrder.TryGetValue(otroValor, out int rivalOrder);
        if (playerOrder == rivalOrder)
        {
            return 0;
        }
        else if (playerOrder > rivalOrder)
            return 2;
        else
            return 1;
    }

    //Para inyectar dependencias con modificadores
    public int CompararValorContraCartaRival(Valor otroValor)
    {
        int result = -1;

        //Solo los modificadores gamma afectan al valor
        Modifier gammaMod = GetMod(ModifierType.Gamma);
        if (gammaMod != null)
            result = gammaMod.CompararValorContraCartaRival(otroValor);

        //-1 solo lo devuelve el metodo virtual de la clase abstracta
        if (result != -1)
            result = this.CompararValor(otroValor);

        return result;
    }

    public int CompararValorEnCartaCante(Valor otroValor)
    {
        int result = -1;

        //Solo los modificadores gamma afectan al valor
        Modifier gammaMod = GetMod(ModifierType.Gamma);
        if (gammaMod != null)
            result = gammaMod.CompararValorEnCartaCante(otroValor);

        //-1 solo lo devuelve el metodo virtual de la clase abstracta
        if (result != -1)
            result = this.CompararValor(otroValor);

        return result;
    }

    //Método base que añade los puntos de la carta
    //y manda el evento correspondiente de UI
    public void PuntuarCarta(ScoreManager sm)
    {
        sm.puntosJugada += cartaBase.Puntos;
        sm.InvokePointScore(cartaBase.Puntos);
    }

    //Para inyectar dependencias con modificadores
    public void PuntuarCartaBaza(ScoreManager sm)
    {
        this.PuntuarCarta(sm);

        foreach (Modifier mod in modifiers)
            mod.PuntuarCartaBaza(sm);
    }
    
    public void PuntuarCartaCante(ScoreManager sm)
    {
        this.PuntuarCarta(sm);

        foreach (Modifier mod in modifiers)
            mod.PuntuarCartaCante(sm);
    }

    public void PuntuarCartaRival(ScoreManager sm)
    {
        this.PuntuarCarta(sm);

        foreach (Modifier mod in modifiers)
            mod.PuntuarCartaRival(sm);
    }

    public void PuntuarCartaMano(ScoreManager sm)
    {
        //Por defecto, las cartas en mano no puntuan

        foreach (Modifier mod in modifiers)
            mod.PuntuarCartaMano(sm);
    }

    public void DescartarCarta(ScoreManager sm)
    {
        //mover al spot de descartes

        foreach (Modifier mod in modifiers)
            mod.DescartarCarta(sm);
    }
}
