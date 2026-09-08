using UnityEngine;

public class M_Fortune : Modifier
{
    float prob = 1f / 7;
    int puntos = 77;
    int valor = 7;
    int bonus = 777;
    public M_Fortune()
    {
        this.Name = "Fortuna";
        this.Description = ct.ChanceText(prob) + " de " + ct.ValueText(valor) + ", " 
            + ct.PointsText(puntos) + " o " + ct.BonusText(bonus);
        this.Type = ModifierType.Alpha;
        this.Index = 3;
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