using UnityEngine;

public abstract class Modifier
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ModifierType Type { get; set; }

    public virtual void OnCardScore(ScoreManager sm)
    {

    }
}
