using System;
using UnityEngine;

public class M_Relief : Modifier
{
    public M_Relief()
    {
        this.Name = "Relevo";
        this.Description = "Al descartar esta carta, roba la carta de la muestra y sustitúyela por esta";
        this.Type = ModifierType.Gamma;
        this.Index = 16;
    }
    public override bool ModificadorAplicable(CardInstance card)
    {
        return card.cartaBase.Valor == Valor.Siete;
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        CardInstance muestra = sm.muestraSpot.cardList[0];
        sm.muestraSpot.ClearSpot();
        //Añade la carta de la muestra a la mano
        sm.deckManager.InitDrawnCard(sm.manoSpot, muestra);
        //Evita que la carta se descarte
        sm.canteSpot.RemoveCard(FatherCard);
        sm.discardedCards.Remove(FatherCard);
        //Y se añade al spot de la muestra
        sm.muestraSpot.AddCard(FatherCard);
    }
}
