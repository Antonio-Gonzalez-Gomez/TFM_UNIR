using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class HoverInfo
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Vector2 Position { get; set; }
    public List<string> AdditionalTitle { get; set; }
    public List<string> AdditionalDescription { get; set; }
    

    public HoverInfo(CardInstance card)
    {
        ColorText ct = new ColorText();

        this.Title = card.cartaBase.StringNombre();
        this.Description = ct.PointsText(card.puntos);
        this.Position = card.transform.position + new Vector3(0, 2.5f, 0);
        this.AdditionalTitle = new List<string>();
        this.AdditionalDescription = new List<string>();

        Modifier alpha = card.GetMod(ModifierType.Alpha);
        Modifier beta = card.GetMod(ModifierType.Beta);
        Modifier gamma = card.GetMod(ModifierType.Gamma);

        if (alpha != null)
        {
            this.Description += "\n" + ct.Color(alpha.Name, "alpha");
            this.AdditionalTitle.Add(alpha.Name);
            this.AdditionalDescription.Add(alpha.Description);
        }

        if (beta != null)
        {
            this.Description += "\n" + ct.Color(beta.Name, "beta");
            this.AdditionalTitle.Add(beta.Name);
            this.AdditionalDescription.Add(beta.Description);
        }

        if (gamma != null)
        {
            this.Description += "\n" + ct.Color(gamma.Name, "gamma");
            this.AdditionalTitle.Add(gamma.Name);
            this.AdditionalDescription.Add(gamma.Description);
        }
    }
}
