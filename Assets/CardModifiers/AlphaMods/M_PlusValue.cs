using UnityEngine;

public class M_PlusValue : Modifier
{
    public M_PlusValue()
    {
        this.Name = "Aumento de valor";
        this.Description = "+ 3 al valor";
        this.Type = ModifierType.Alpha;
    }

    public override void OnCardScore(ScoreManager sm)
    {
        sm.valorJugada += 3;
        sm.InvokeValueScore(3);
    }
}
