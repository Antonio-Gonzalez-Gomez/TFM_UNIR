using UnityEngine;

public class M_Strength : Modifier
{
    public M_Strength()
    {
        this.power = 30;
        this.Name = "Fuerza";
        this.Description = "+ " + power.ToString() + " puntos";
        this.Type = ModifierType.Alpha;
        this.Index = 0;
    }

    public override void AntesDePuntuarCarta(ScoreManager sm, string posicion)
    {
        FatherCard.puntos += power;
    }
}
