using System;
using System.Threading.Tasks;
using UnityEngine;

public class M_Summon : Modifier
{
    public M_Summon()
    {
        this.Name = "Convocatoria";
        this.Description = "Al descartar esta carta, roba una carta de figura del mismo palo";
        this.Type = ModifierType.Beta;
        this.Index = 11;
    }
    public override bool ModificadorAplicable(CardInstance card)
    {
        //Solo aplicable a cartas numéricas
        return !card.cartaBase.EsCartaFigura();
    }

    public override async Task EfectoCartaDescartada(ScoreManager sm)
    {
        //Comprobación de cartas en el mazo
        if (sm.mazoRobarSpot.cardList.Count == 0)
        {
            return;
        }

        try
        {
            CardInstance res = sm.mazoRobarSpot.cardList.Find(x => x.CompararPalo(ParentReference.cartaBase.Palo) && x.cartaBase.EsCartaFigura());
            sm.deckManager.InitDrawnCard(sm.manoSpot, res);
            await Task.CompletedTask;
        }
        catch (NullReferenceException)
        {
            return;
        }
    }
}
