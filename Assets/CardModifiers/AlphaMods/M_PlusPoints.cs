using UnityEngine;

public class M_PlusPoints : Modifier
{
    public M_PlusPoints()
    {
        this.Name = "Fuerza";
        this.Description = "+ 30 puntos al puntuar en baza o cante";
        this.Type = ModifierType.Alpha;
    }

    private void Activate(ScoreManager sm)
    {
        sm.puntosJugada += 30;
        sm.InvokePointScore(30);
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
