using System;
using UnityEngine;

public class M_Switch : Modifier
{
    public M_Switch()
    {
        this.Name = "Cambiazo";
        this.Description = "Al jugar esta carta como baza, intercámbiala con la del rival (solo válido para 6)";
        this.Type = ModifierType.Gamma;
        this.Index = 15;
    }
    public override bool ModificadorAplicable(CardInstance card)
    {
        return card.cartaBase.Valor == Valor.Seis;
    }


    public override int CompararContraCartaRival(CardInstance rival, Palo paloMuestra)
    {
        //Se revisa el modificador gamma de la carta del rival
        Modifier gammaMod = rival.GetMod(ModifierType.Gamma);
        //En caso de dos cartas con este modificador, habría un bucle infinito
        //Pero si se permite el efecto de otros mods gamma (M_Counter)
        if (gammaMod != null && gammaMod.Name != this.Name)
        {
            int res = gammaMod.CompararContraCartaRival(FatherCard, paloMuestra);
            if (res != -1)
            {
                return res;
            }
        }

        //Al haber intercambiado las cartas, que gane la carta del rival == que gane el jugador
        Palo paloPlayer = rival.cartaBase.Palo;
        Palo paloRival = FatherCard.cartaBase.Palo;

        if (paloPlayer != paloRival)
        {
            //Si una de las cartas es la muestra, gana la baza
            if (paloPlayer == paloMuestra)
            {
                return 0;
            }

            else
            {
                return 1;
            }
        }

        else
        {
            return rival.CompararValor(FatherCard.cartaBase.Valor) == 1 ? 0 : 1;
        }
    }

    public override void AntesDePuntuarCarta(ScoreManager sm, string posicion)
    {
        //La carta del jugador se intercambia
        if (posicion == "baza")
        {
            CardInstance cartaPlayer = sm.bazaSpot.cardList[0];
            CardInstance cartaRival = sm.rivalSpot.cardList[0];

            sm.bazaSpot.ClearSpot();
            sm.rivalSpot.ClearSpot();

            sm.bazaSpot.AddCard(cartaRival);
            sm.rivalSpot.AddCard(cartaPlayer);

            //Esto evita que la carta del jugador se puntúe ahora
            //Se puntuará después de las cartas del cante (replay se resetea)
            FatherCard.replay = 0;

            //Esto permite que la carta del rival se puntúe como baza
            cartaRival.PuntuarCarta(sm, "baza");
        }
    }
}
