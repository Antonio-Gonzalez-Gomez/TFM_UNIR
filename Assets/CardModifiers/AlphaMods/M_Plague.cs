using UnityEngine;

public class M_Plague : Modifier
{
    public M_Plague()
    {
        this.points = 1;
        this.Name = "Plaga";
        this.Description = "+ " + points.ToString() + " al valor por cada carta descartada con este modificador esta baza";
        this.Type = ModifierType.Alpha;
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        int contadorPlaga = sm.discardedCards.FindAll(x =>
            x.GetMod(ModifierType.Alpha).Name == "Plaga").Count;
        //Por defecto, incValor = contadorPlaga
        int incValor = points * contadorPlaga;
        sm.valorJugada += incValor;
        sm.InvokeValueScore(incValor);
    }
}
