using System.Threading.Tasks;
using UnityEngine;

public class M_Solitude : Modifier
{
    public M_Solitude()
    {
        this.power = 150;
        this.Name = "Soledad";
        this.Description = "No puede formar cantes, " + ct.BonusText(power) + " al descartar";
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

    public override async Task EfectoCartaDescartada(ScoreManager sm)
    {
        //No puntua si pierde
        await base.EfectoCartaDescartada(sm);

        sm.bonusJugada += power;
        await sm.InvokeBonusScore(ParentReference.drag, power, false);
    }
}
