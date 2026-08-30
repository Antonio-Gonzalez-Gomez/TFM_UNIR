using UnityEngine;

public class M_Fortune : Modifier
{
    float prob = 1 / 7;
    int puntos = 77;
    int valor = 7;
    int bonus = 777;
    public M_Fortune()
    {
        this.Name = "Fortuna";
        this.Description = "Probabilidad de 1/7 de recibir "
            + puntos.ToString() + " puntos, " + valor.ToString() + " al valor o " + bonus.ToString() + " puntos bonus";
        this.Type = ModifierType.Alpha;
    }

    public override void Activate(ScoreManager sm)
    {
        //Son 3 llamadas independientes al RNG
        if (RNG.RandomRange(prob))
        {
            sm.puntosJugada += puntos;
            sm.InvokeValueScore(puntos);
        }

        if (RNG.RandomRange(prob))
        {
            sm.valorJugada += valor;
            sm.InvokeValueScore(valor);
        }

        if (RNG.RandomRange(prob))
        {
            sm.bonusJugada += bonus;
            sm.InvokeValueScore(bonus);
        }
    }
}