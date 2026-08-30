using System;
using UnityEngine;

public class M_Invitation : Modifier
{
    public M_Invitation()
    {
        this.Name = "Invitación";
        this.Description = "Al descartar esta carta, roba una carta de figura del mismo palo";
        this.Type = ModifierType.Beta;
    }
    public override bool ModificadorAplicable(CardInstance card)
    {
        //Solo aplicable a cartas numéricas
        return !card.cartaBase.EsCartaFigura();
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        //Comprobación de cartas en el mazo
        if (sm.mazoRobarSpot.cardList.Count == 0)
        {
            return;
        }

        try
        {
            CardInstance res = sm.mazoRobarSpot.cardList.Find(x => x.CompararPalo(Palo.Oros) && x.cartaBase.EsCartaFigura());
            sm.deckManager.InitDrawnCard(sm.manoSpot, res);
        }
        catch (NullReferenceException)
        {
            return;
        }
    }
}
