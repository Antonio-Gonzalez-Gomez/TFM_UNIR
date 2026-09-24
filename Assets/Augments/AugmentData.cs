using System.Threading.Tasks;
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
    public virtual async Task PuntuarCarta(CardInstance card, ScoreManager sm, string posicion)
    {
        await Task.CompletedTask;
    }

    public virtual async Task PuntuarCante(Cante cante, ScoreManager sm)
    {
        await Task.CompletedTask;
    }
}
