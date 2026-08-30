using UnityEngine;

public class M_Recycling : Modifier
{
    public M_Recycling()
    {
        this.points = 5;
        this.Name = "Reciclaje";
        this.Description = "Elimina esta carta del mazo al ser descartada " + points.ToString() + " veces";
        this.Type = ModifierType.Beta;
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        points--;
        if (points == 0)
        {
            FatherCard.DestroyCard();
        }
    }
}
