using UnityEngine;

public abstract class AugmentData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public AugmentRarity Rarity { get; set; }
    //TODO: es necesario el Index si no hay sprites?
    public int Index { get; set; }

    protected ColorText ct = new ColorText();

    public AugmentInstance ParentReference { get; set; }
    public virtual void PuntuarCarta(CardInstance card, ScoreManager sm, string posicion)
    {

    }

    public virtual void PuntuarCante(Cante cante, ScoreManager sm)
    {

    }
}
