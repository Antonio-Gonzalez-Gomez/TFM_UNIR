using System.Threading.Tasks;
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
        //TODO: Algun efectillo
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

    public override async Task EfectoCartaDescartada(ScoreManager sm)
    {
        ResetMod();
        await Task.CompletedTask;
    }

    public override async Task EfectoCartaMano(ScoreManager sm)
    {
        sm.bonusJugada += totalBonus;
        await sm.InvokeBonusScore(ParentReference.drag, totalBonus, false);

        totalBonus += power;
    }
}
