using UnityEngine;

public class M_Hunting : Modifier
{
    private int totalValue = 1;
    public M_Hunting()
    {
        this.power = 1;
        this.Name = "Cacería";
        this.Description = "+ " + totalValue.ToString() + " al valor al ganar la baza con esta carta,\n" +
            "que aumenta en " + power.ToString() + " cada vez que este efecto se active.";
        this.Type = ModifierType.Beta;
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
