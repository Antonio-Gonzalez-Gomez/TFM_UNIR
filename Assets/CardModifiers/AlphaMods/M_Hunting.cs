using UnityEngine;

public class M_Hunting : Modifier
{
    private int totalValue = 0;
    public M_Hunting()
    {
        this.points = 1;
        this.Name = "Cacería";
        this.Description = "+ " + totalValue.ToString() + " al valor al ganar la baza con esta carta,\n" +
            "que aumenta en " + points.ToString() + " cada vez que este efecto se active.";
        this.Type = ModifierType.Alpha;
    }

    public override void PuntuarCartaBaza(ScoreManager sm)
    {
        totalValue += points;

        sm.valorJugada += totalValue;
        sm.InvokeValueScore(totalValue);
    }
}
