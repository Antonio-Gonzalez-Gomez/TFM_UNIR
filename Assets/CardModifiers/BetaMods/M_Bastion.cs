using UnityEngine;

public class M_Bastion : Modifier
{
    private int totalBonus = 0;
    public M_Bastion()
    {
        this.power = 30;
        this.Name = "Bastión";
        this.Description = "+ " + totalBonus.ToString() + " puntos bonus al ganar una baza con esta carta en mano,\n" +
            "que incrementa en " + power.ToString() + " cada vez que este efecto se active\n" +
            "(el bonus se resetea al jugar la carta)";
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
        totalBonus += power;

        sm.bonusJugada += totalBonus;
        sm.InvokeBonusScore(totalBonus);
    }
}
