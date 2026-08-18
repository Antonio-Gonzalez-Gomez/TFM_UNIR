using UnityEngine;

public class M_PlusPoints : Modifier
{
    public M_PlusPoints()
    {
        this.Name = "Aumento de puntos";
        this.Description = "+ 30 puntos";
        this.Type = ModifierType.Alpha;
    }

    public override void OnCardScore(ScoreManager sm)
    {
        sm.puntosJugada += 30;
        sm.InvokePointScore(30);
    }
}
