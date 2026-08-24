using UnityEngine;

public class M_Dexterity : Modifier
{
    public M_Dexterity()
    {
        this.points = 3;
        this.Name = "Destreza";
        this.Description = "+ " + points.ToString() + " al valor al puntuar en baza o cante";
        this.Type = ModifierType.Alpha;
    }

    public void Activate(ScoreManager sm)
    {
        sm.valorJugada += points;
        sm.InvokeValueScore(points);
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
