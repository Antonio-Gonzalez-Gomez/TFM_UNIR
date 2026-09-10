using UnityEngine;

public class M_Dexterity : Modifier
{
    public M_Dexterity()
    {
        this.power = 3;
        this.Name = "Destreza";
        this.Description = ct.ValueText(power);
        this.Type = ModifierType.Alpha;
        this.Index = 1;
    }

    public override async void Activate(ScoreManager sm)
    {
        sm.valorJugada += power;
        await sm.InvokeValueScore(FatherCard.drag, power);
    }
}
