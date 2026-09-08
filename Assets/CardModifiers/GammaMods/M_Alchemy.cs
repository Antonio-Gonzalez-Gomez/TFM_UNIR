using UnityEngine;

public class M_Alchemy : Modifier
{
    float valueRate = 1f / 4;
    public M_Alchemy(int cardPoints)
    {
        this.power = Mathf.CeilToInt(cardPoints * valueRate);
        this.Name = "Transmutación";
        this.Description = "Convierte " + ct.fractions[valueRate] + " de los " +
            ct.Color("puntos", "points") + " de esta carta en " + ct.Color("valor", "value");
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
