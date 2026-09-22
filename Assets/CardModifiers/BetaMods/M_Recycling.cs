using UnityEngine;

public class M_Recycling : Modifier
{
    public M_Recycling()
    {
        this.power = 5;
        this.Name = "Reciclaje";
        this.Description = "Elimina esta carta del mazo al ser descartada " + power.ToString() + " veces";
        this.Type = ModifierType.Beta;
        this.Index = 9;
    }

    public override void EfectoCartaDescartada(ScoreManager sm)
    {
        power--;
        if (power == 0)
        {
            ParentReference.DestroyCard();
        }
    }
}
