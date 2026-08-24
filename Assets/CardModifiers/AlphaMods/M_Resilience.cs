using UnityEngine;

public class M_Resilience : Modifier
{
    private int totalBonus = 0;
    public M_Resilience()
    {
        this.points = 30;
        this.Name = "Resiliencia";
        this.Description = "+ " + totalBonus.ToString() + " puntos bonus al ganar una baza con esta carta en mano,\n" +
            "que incrementa en " + points.ToString() + " cada vez que este efecto se active\n" +
            "(el bonus se resetea al jugar la carta)";
        this.Type = ModifierType.Alpha;
    }

    private void ResetMod()
    {
        //Algun efectillo
        Debug.Log("Resiliencia reset!");
        totalBonus = 0;
    }
    public override void PuntuarCartaBaza(ScoreManager sm)
    {
        base.PuntuarCartaBaza(sm);
    }

    public override void PuntuarCartaCante(ScoreManager sm)
    {
        base.PuntuarCartaBaza(sm);
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        ResetMod();
    }

    public override void EfectoCartaMano(ScoreManager sm)
    {
        totalBonus += points;

        sm.bonusJugada += totalBonus;
        sm.InvokeBonusScore(totalBonus);
    }
}
