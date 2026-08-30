using UnityEngine;

public class M_Coinflip : Modifier
{
    float prob = 1 / 2;
    public M_Coinflip()
    {
        this.points = 2;
        this.Name = "Moneda";
        this.Description = "Al puntuar en baza o cante, probabilidad de 1/2 de o bien duplicar el valor del cante o de restar los puntos de esta carta";
        this.Type = ModifierType.Alpha;
    }

    private void Activate(ScoreManager sm)
    {
        if (RNG.RandomRange(prob))
        {
            //Restar puntos de la carta
            int puntos = FatherCard.cartaBase.Puntos;
            sm.puntosJugada -= puntos;
            sm.InvokePointScore(-puntos);
        }
        else
        {
            //Duplicar el valor del cante
            int valor = 2 * sm.valorJugada;
            sm.valorJugada = valor;
            sm.InvokeValueScore(valor);
        }
    }

    public override void PuntuarCarta(ScoreManager sm, string posicion)
    {
        switch (posicion)
        {
            case "baza":
                Activate(sm);
                break;

            case "cante":
                Activate(sm);
                break;

            default:
                break;
        }
    }
}
