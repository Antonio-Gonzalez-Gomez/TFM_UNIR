using UnityEngine;

public class M_PlusValue : Modifier
{
    public M_PlusValue()
    {
        this.Name = "Destreza";
        this.Description = "+ 3 al valor al puntuar en baza o cante";
        this.Type = ModifierType.Alpha;
    }

    public void Activate(ScoreManager sm)
    {
        sm.valorJugada += 3;
        sm.InvokeValueScore(3);
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
