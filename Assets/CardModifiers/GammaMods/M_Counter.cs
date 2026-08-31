using UnityEngine;

public class M_Counter : Modifier
{
    private bool contraActivada = false;
    public M_Counter()
    {
        this.power = 31;
        this.Name = "Contra";
        this.Description = "+" + power.ToString() + " puntos y gana la baza al jugar contra cualquier as o tres (solo válido para 2 y 4)";
        this.Type = ModifierType.Gamma;
    }

    public override bool ModificadorAplicable(CardInstance card)
    {
        return card.cartaBase.Valor == Valor.Dos || card.cartaBase.Valor == Valor.Cuatro;
    }

    public override int CompararContraCartaRival(CardInstance rival, Palo paloMuestra)
    {
        if (rival.cartaBase.Valor == Valor.As || rival.cartaBase.Valor == Valor.Tres)
        {
            contraActivada = true;
            //0 == ganar la baza
            return 0;
        }

        contraActivada = false;
        return -1;
    }

    public override void AntesDePuntuarCarta(ScoreManager sm, string posicion)
    {
        //Puede ocurrir que el bool quede a true y luego la carta puntúe en otro lado (carta del rival)
        if (contraActivada && sm.bazaSpot.cardList.Contains(FatherCard))
            FatherCard.puntos += power;
    }

}
