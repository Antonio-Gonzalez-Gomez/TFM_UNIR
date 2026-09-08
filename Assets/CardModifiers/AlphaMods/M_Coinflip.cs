using UnityEngine;

public class M_Coinflip : Modifier
{
    float prob = 1f / 2;
    public M_Coinflip()
    {
        this.power = 2;
        this.Name = "Cara o Cruz";
        this.Description = "Duplica el " + ct.Color("valor del cante", "value") + ", pero con " + ct.ChanceText(prob) + " de no puntuar";
        this.Type = ModifierType.Alpha;
        this.Index = 4;
    }

    public override void Activate(ScoreManager sm)
    {
        //Duplicar el valor del cante
        int valor = power * sm.valorJugada;
        sm.valorJugada = valor;
        sm.InvokeValueScore(valor);
    }

    public override void AntesDePuntuarCarta(ScoreManager sm, string posicion)
    {
        if (RNG.RandomRange(prob))
        {
            //Esto evita que la carta puntue directamente
            FatherCard.replay = -9999;
        }
    }
}
