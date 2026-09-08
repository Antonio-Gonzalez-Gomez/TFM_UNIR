using Unity.VisualScripting;
using UnityEngine;

public class M_Mirror : Modifier
{
    private CardData original;
    public M_Mirror(CardData cartaOriginal)
    {
        original = cartaOriginal;
        this.Name = "Espejo";
        this.Description = "Al jugar como cante, copia el palo y valor de la carta del cante a la izquierda de esta";
        this.Type = ModifierType.Gamma;
        this.Index = 14;
    }
    public override bool ModificadorAplicable(CardInstance card)
    {
        return card.cartaBase.Valor == Valor.Cinco;
    }

    public override void AntesDeEvaluarCante(ScoreManager sm)
    {
        int currentCardIndex = sm.canteSpot.cardList.IndexOf(FatherCard);
        //Si hay carta a la izquierda, se copia su palo/valor
        if (currentCardIndex != 0)
        {
            FatherCard.cartaBase = sm.canteSpot.cardList[--currentCardIndex].cartaBase;
        }

    }

    public override void DespuesDePuntuarCarta(ScoreManager sm)
    {
        FatherCard.cartaBase = original;
    }

}
