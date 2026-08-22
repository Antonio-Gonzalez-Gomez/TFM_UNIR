using UnityEngine;

public class M_PlusBonus : Modifier
{
    public M_PlusBonus()
    {
        //TODO: cambiar este mod por exilio
        this.Name = "Aumento de bonus";
        this.Description = "+ 100 puntos bonus al puntuar en baza o cante";
        this.Type = ModifierType.Alpha;
    }

    public override void PuntuarCartaBaza(ScoreManager sm)
    {
        sm.bonusJugada += 100;
        sm.InvokeBonusScore(100);
    }
    public override void PuntuarCartaCante(ScoreManager sm)
    {
        sm.bonusJugada += 100;
        sm.InvokeBonusScore(100);
    }
}
