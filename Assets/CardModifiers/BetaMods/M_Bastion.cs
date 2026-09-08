using UnityEngine;

public class M_Bastion : Modifier
{
    private int totalBonus = 40;
    public M_Bastion()
    {
        this.power = 40;
        this.Name = "Bastión";
        this.Description = ct.BonusText(totalBonus) + " al ganar una baza con esta carta en mano," +
            "aumenta " + ct.Color(power.ToString(), "bonus") + " cada vez seguida que se active";
        this.Type = ModifierType.Beta;
        this.Index = 7;
    }

    private void ResetMod()
    {
        //Algun efectillo
        Debug.Log("Bastion reset!");
        totalBonus = 0;
    }
    public override void PuntuarCarta(ScoreManager sm, string posicion)
    {
        switch (posicion)
        {
            case "baza":
                ResetMod();
                break;

            case "cante":
                ResetMod();
                break;

            default:
                break;
        }
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        ResetMod();
    }

    public override void EfectoCartaMano(ScoreManager sm)
    {
        sm.bonusJugada += totalBonus;
        sm.InvokeBonusScore(totalBonus);

        totalBonus += power;
    }
}
