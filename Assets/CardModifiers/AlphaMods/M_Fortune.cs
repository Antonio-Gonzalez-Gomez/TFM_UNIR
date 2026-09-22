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
            await sm.InvokePointScore(ParentReference.drag, puntos, false);
        }

        if (RNG.RandomRange(prob))
        {
            sm.valorJugada += valor;
            await sm.InvokeValueScore(ParentReference.drag, valor, false);
        }

        if (RNG.RandomRange(prob))
        {
            sm.bonusJugada += bonus;
            await sm.InvokeBonusScore(ParentReference.drag, bonus, false);
        }
    }
}