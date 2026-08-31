using UnityEngine;
using static UnityEngine.UI.Image;

public class M_Encore : Modifier
{
    public M_Encore()
    {
        this.Name = "Réplica";
        this.Description = "Al puntuar como cante, puntúa otra vez (solo válido para cartas de figura)";
        this.Type = ModifierType.Gamma;
    }

    public override bool ModificadorAplicable(CardInstance card)
    {
        return card.cartaBase.EsCartaFigura();
    }

    public override void AntesDePuntuarCarta(ScoreManager sm, string posicion)
    {
        if (posicion == "cante")
            FatherCard.replay++;
    }
}
