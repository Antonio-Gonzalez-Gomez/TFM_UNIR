using UnityEngine;

public class M_Alchemy : Modifier
{
    float valueRate = 1 / 4;
    public M_Alchemy()
    {
        this.power = Mathf.CeilToInt(FatherCard.puntos * valueRate);
        this.Name = "Transmutación";
        this.Description = "Los puntos de esta carta se convierten en +" + power.ToString() + " al valor (1/4 de los puntos originales, solo válido para 1 y 3)";
        this.Type = ModifierType.Gamma;
        this.Index = 12;
    }

    public override bool ModificadorAplicable(CardInstance card)
    {
        return card.cartaBase.Valor == Valor.As || card.cartaBase.Valor == Valor.Tres;
    }

    public override void AntesDePuntuarCarta(ScoreManager sm, string posicion)
    {
        //Es necesario actualizar este valor ya que los puntos pueden variar al añadir o eliminar modificadores a la carta
        this.power = Mathf.CeilToInt(FatherCard.puntos * valueRate);
        FatherCard.puntos = 0;
    }

    public override void Activate(ScoreManager sm)
    {
        sm.valorJugada = power;
        sm.InvokeValueScore(power);
    }



}
