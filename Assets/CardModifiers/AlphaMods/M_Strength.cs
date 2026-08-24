using UnityEngine;

public class M_Strength : Modifier
{
    public M_Strength()
    {
        this.points = 30;
        this.Name = "Fuerza";
        this.Description = "+ " + points.ToString() + " puntos al puntuar en baza o cante";
        this.Type = ModifierType.Alpha;
    }

    private void Activate(ScoreManager sm)
    {
        sm.puntosJugada += points;
        sm.InvokePointScore(points);
    }

    public override void PuntuarCartaBaza(ScoreManager sm)
    {
        Activate(sm);
    }
    public override void PuntuarCartaCante(ScoreManager sm)
    {
        Activate(sm);
    }
}
