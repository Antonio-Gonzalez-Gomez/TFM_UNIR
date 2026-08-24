using UnityEngine;

public class M_Exile : Modifier
{
    public M_Exile()
    {
        this.points = 150;
        this.Name = "Exilio";
        this.Description = "No puede formar cantes, + " + points.ToString() + " puntos bonus al descartar";
        this.Type = ModifierType.Alpha;
    }
    public override bool ModificadorAplicable(CardInstance card)
    {
        Valor valorBase = card.cartaBase.Valor;
        //En este caso, la comparacion se hace de forma directa con la carta base para evitar
        //Que otro modificador/aumento afecte al resultado
        if (valorBase == Valor.Sota || valorBase == Valor.Caballo || valorBase == Valor.Rey)
            return true;
        else
            return false;
    }
    public override int CompararValorEnCartaCante(Valor otroValor)
    {
        //Siempre es distinta a las sotas, caballos y reyes, impidiendo que forme cantes
        return 1;
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        sm.bonusJugada += points;
        sm.InvokeBonusScore(points);
    }
}
