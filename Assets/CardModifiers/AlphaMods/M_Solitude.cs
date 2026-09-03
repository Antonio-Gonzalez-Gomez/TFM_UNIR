using UnityEngine;

public class M_Solitude : Modifier
{
    public M_Solitude()
    {
        this.power = 150;
        this.Name = "Soledad";
        this.Description = "No puede formar cantes, + " + power.ToString() + " puntos bonus al descartar (solo válido para cartas de figura)";
        this.Type = ModifierType.Alpha;
        this.Index = 5;
    }
    public override bool ModificadorAplicable(CardInstance card)
    {
        return card.cartaBase.EsCartaFigura();
    }
    public override int CompararValorEnCartaCante(Valor otroValor)
    {
        //Siempre es distinta a las sotas, caballos y reyes, impidiendo que forme cantes
        return 1;
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        base.EfectoCartaDescartada(sm);

        sm.bonusJugada += power;
        sm.InvokeBonusScore(power);
    }
}
