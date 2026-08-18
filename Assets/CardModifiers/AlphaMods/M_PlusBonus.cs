using UnityEngine;

public class M_PlusBonus : Modifier
{
    public M_PlusBonus()
    {
        this.Name = "Aumento de bonus";
        this.Description = "+ 100 puntos bonus";
        this.Type = ModifierType.Alpha;
    }

    public override void OnCardScore(ScoreManager sm)
    {
        sm.bonusJugada += 100;
        sm.InvokeBonusScore(100);
    }
}
