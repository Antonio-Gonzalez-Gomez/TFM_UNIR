using UnityEngine;

public class M_Plague : Modifier
{
    public M_Plague()
    {
        this.power = 1;
        this.Name = "Plaga";
        this.Description = ct.ValueText(power) + " por cada carta descartada con este modificador en la baza";
        this.Type = ModifierType.Alpha;
        this.Index = 2;
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        base.EfectoCartaDescartada(sm);

        int contadorPlaga = sm.discardedCards.FindAll(x =>
            x.GetMod(ModifierType.Alpha).Name == "Plaga").Count;
        //Por defecto, incValor = contadorPlaga
        int incValor = power * contadorPlaga;
        sm.valorJugada += incValor;
        sm.InvokeValueScore(incValor);
    }
}
