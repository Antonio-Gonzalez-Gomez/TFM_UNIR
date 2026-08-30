using UnityEngine;

public class M_Coinflip : Modifier
{
    float prob = 1 / 2;
    public M_Coinflip()
    {
        this.power = 2;
        this.Name = "Moneda";
        this.Description = "Duplica el valor del cante, pero con una probabilidad de 1/2 de no puntuar";
        this.Type = ModifierType.Alpha;
    }

    public override void Activate(ScoreManager sm)
    {
        //Duplicar el valor del cante
        int valor = power * sm.valorJugada;
        sm.valorJugada = valor;
        sm.InvokeValueScore(valor);
    }

    public override void AntesDePuntuarCarta(ScoreManager sm)
    {
        if (RNG.RandomRange(prob))
        {
            //Esto evita que la carta puntue directamente
            FatherCard.replay = -9999;
        }
    }
}
