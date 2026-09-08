using UnityEngine;

public class M_Hunter : Modifier
{
    private int totalValue = 1;
    public M_Hunter()
    {
        this.power = 1;
        this.Name = "Cazador";
        this.Description = ct.ValueText(totalValue) + " al ganar la baza con esta carta," +
            "aumenta " + ct.Color(power.ToString(), "value") + " cada vez que se active.";
        this.Type = ModifierType.Beta;
        this.Index = 6;
    }

    public override void PuntuarCarta(ScoreManager sm, string posicion)
    {
        switch (posicion)
        {
            case "baza":
                sm.valorJugada += totalValue;
                sm.InvokeValueScore(totalValue);

                totalValue += power;
                break;

            default:
                break;
        }
    }
}
