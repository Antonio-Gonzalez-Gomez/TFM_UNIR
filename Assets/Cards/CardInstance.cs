using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardInstance : MonoBehaviour
{
    public CardData cartaBase;

    private List<Modifier> modifiers;
    private SpriteRenderer spriteRenderer;
    public DragController drag;

    private SpriteRenderer alphaModSprite;
    private SpriteRenderer betaModSprite;
    private SpriteRenderer gammaModSprite;
    //Indice del sprite con el reverso de las cartas
    private const int reversoIndex = 41;
    //Estos valores son dinámicos (se espera que los modificadores/aumentos los modifiquen)
    //Número de veces que la carta es puntuada
    public int replay = 1;
    //Puntos de la carta
    public int puntos = 0;

    private void Awake()
    {
        modifiers = new List<Modifier>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        drag = GetComponent<DragController>();

        //AQUI ESTA EL PROBLEMA
        SpriteRenderer[] modSprites = GetComponentsInChildren<SpriteRenderer>();
        alphaModSprite = modSprites.First(x => x.name == "ModAlphaSprite");
        betaModSprite = modSprites.First(x => x.name == "ModBetaSprite");
        gammaModSprite = modSprites.First(x => x.name == "ModGammaSprite");
    }

    //Inicializa la carta con el sprite del reverso (para que quede sobre el mazo)
    public void InitCard(CardData cartaBase)
    {
        this.cartaBase = cartaBase;
        puntos = cartaBase.Puntos;
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

        //Se actualizan los sprites de los modificadores
        alphaModSprite.sprite = TryGetModSprite(ModifierType.Alpha);
        betaModSprite.sprite = TryGetModSprite(ModifierType.Beta);
        gammaModSprite.sprite = TryGetModSprite(ModifierType.Gamma);
    }

    private Sprite TryGetModSprite(ModifierType type)
    {
        Modifier mod = GetMod(type);
        //Si no hay mod de ese tipo, no habra sprite
        if (mod == null)
            return null;
        
        return SpriteSelector.GetModSpriteByIndex(mod.Index);
    }

    public Modifier GetMod(ModifierType type)
    {
        try
        {
            return modifiers.Find(x => x.Type == type);
        }
        catch(NullReferenceException)
        {
            return null;
        }
    }

    //Añade un modificador, sustituyendo el anterior del mismo tipo
    public void AddModifier(Modifier mod)
    {
        Modifier alphaMod = mod.Type == ModifierType.Alpha ? mod : GetMod(ModifierType.Alpha);
        Modifier betaMod = mod.Type == ModifierType.Beta ? mod : GetMod(ModifierType.Beta);
        Modifier gammaMod = mod.Type == ModifierType.Gamma ? mod : GetMod(ModifierType.Gamma);

        //Es necesario rehacer la lista de modificadores para que guarden el orden correcto
        modifiers = new List<Modifier>();
        if (alphaMod != null && alphaMod.ModificadorAplicable(this))
        {
            alphaMod.FatherCard = this;
            modifiers.Add(alphaMod);
        }
        if (betaMod != null && betaMod.ModificadorAplicable(this))
        {
            betaMod.FatherCard = this;
            modifiers.Add(betaMod);
        }
        if (gammaMod != null && gammaMod.ModificadorAplicable(this))
        {
            gammaMod.FatherCard = this;
            modifiers.Add(gammaMod);
        }
        //Se actualizan los sprites de la carta
        UpdateSprite();
    }
    public void DestroyCard()
    {
        //TODO: es necesario borrar de los spots y demas listas de DeckManager/ScoreManager?

        Destroy(this);
    }

    //Método base que compara dos palos
    public bool CompararPalo(Palo otroPalo)
    {
        if (this.cartaBase.Palo == otroPalo)
            return true;

        return false;
    }

    //Para inyectar dependencias con modificadores
    public bool CompararPaloConCartaCante(CardInstance otraCarta)
    {
        //Se usa un int para comprobar si algun mod modifica el valor
        int result = -1;

        //Solo los mods beta (M_Polivalence) afectan al palo en este caso
        Modifier betaMod = GetMod(ModifierType.Beta);
        if (betaMod != null)
            result = betaMod.CompararPaloConCartaCante(otraCarta);

        //Se comprueba el modificador de la otra carta
        if (result == -1)
        {
            Modifier otroMod = otraCarta.GetMod(ModifierType.Beta);
            if (otroMod != null)
                result = otroMod.CompararPaloConCartaCante(this);
        }

        //Si sigue valiendo -1, entonces ambos mods no afectan al palo
        if (result == -1)
            result = CompararPalo(otraCarta.cartaBase.Palo) ? 0 : 1;

        return result == 0 ? true : false;
    }

    public bool CompararPaloConMuestraCante(Palo muestra)
    {
        //Se usa un int para comprobar si algun mod modifica el valor
        int result = -1;

        Modifier betaMod = GetMod(ModifierType.Beta);
        if (betaMod != null)
            result = betaMod.CompararPaloConMuestraCante(muestra);

        //Si sigue valiendo -1, entonces el mod no afecta al palo
        if (result == -1)
            result = CompararPalo(muestra) ? 0 : 1;

        return result == 0 ? true : false;
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

    /// En caso de que haya modificadores de distintos tipo que afecten al valor de la carta,
    /// seria posible recorrer la lista y buscar la primera ocurrencia de valor distinto a -1.
    /// Esto daría prioridad a los modificadores Alpha (o los Gamma si se recorre al revés) sobre dichos efectos.
    /// Intentar que varios modificadores afecten al valor de forma simultánea es posible pero
    /// requeriría refactorizar todo al respecto para que los modificadores no se fijen en la carta base.

    public int CompararValorEnCartaCante(Valor otroValor)
    {
        int result = -1;

        //Solo los modificadores alpha (M_Solitude) afectan al valor en este caso
        Modifier alphaMod = GetMod(ModifierType.Alpha);
        if (alphaMod != null)
            result = alphaMod.CompararValorEnCartaCante(otroValor);

        //-1 solo lo devuelve el metodo virtual de la clase abstracta
        if (result == -1)
            result = this.CompararValor(otroValor);

        return result;
    }

    //Método que añade a la puntuación del cante los puntos de la carta
    //Además de invocar a los modificadores/aumentos correspondientes
    //Y mandar el evento correspondiente de UI
    public void PuntuarCarta(ScoreManager sm, String posicion)
    {
        //Se actualiza el valor de replay u otros efectos previos a la puntuacion
        foreach (Modifier mod in modifiers)
            mod.AntesDePuntuarCarta(sm, posicion);

        for (int i = 0; i < replay; i++)
        {
            sm.puntosJugada += cartaBase.Puntos;
            sm.InvokePointScore(cartaBase.Puntos);

            foreach (Modifier mod in modifiers)
                mod.PuntuarCarta(sm, posicion);
        }

        foreach (Modifier mod in modifiers)
            mod.DespuesDePuntuarCarta(sm);

        //Se resetea al final para evitar que efectos anteriores al puntuaje de esta carta interfieran
        puntos = cartaBase.Puntos;
        replay = 1;
    }

    public void EfectoCartaMano(ScoreManager sm)
    {
        //Por defecto, las cartas en mano no puntuan

        foreach (Modifier mod in modifiers)
            mod.EfectoCartaMano(sm);
    }

    public void EfectoCartaDescartada(ScoreManager sm)
    {
        //Las cartas descartadas tampoco puntúan

        foreach (Modifier mod in modifiers)
            mod.EfectoCartaDescartada(sm);
    }
}
