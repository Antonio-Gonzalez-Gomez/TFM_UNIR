using UnityEngine;

public class M_Dexterity : Modifier
{
    public M_Dexterity()
    {
        this.power = 3;
        this.Name = "Destreza";
        this.Description = "+ " + power .ToString() + " al valor";
        this.Type = ModifierType.Alpha;
        this.Index = 1;
    }

    public override void Activate(ScoreManager sm)
    {
        sm.valorJugada += power;
        sm.InvokeValueScore(power);
    }
}
