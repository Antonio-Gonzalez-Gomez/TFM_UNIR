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

    public override async void Activate(ScoreManager sm)
    {
        //Son 3 llamadas independientes al RNG
        if (RNG.RandomRange(prob))
        {
            sm.puntosJugada += puntos;
            await sm.InvokePointScore(FatherCard.drag, puntos);
        }

        if (RNG.RandomRange(prob))
        {
            sm.valorJugada += valor;
            await sm.InvokeValueScore(FatherCard.drag, valor);
        }

        if (RNG.RandomRange(prob))
        {
            sm.bonusJugada += bonus;
            await sm.InvokeBonusScore(FatherCard.drag, bonus);
        }
    }
}