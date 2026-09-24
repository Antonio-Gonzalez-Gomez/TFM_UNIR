using System.Collections.Generic;

public class ColorText
{
    private Dictionary<string, string> dict;
    //No hay una forma comoda de hacer esto
    public Dictionary<float, string> fractions;
    public ColorText()
    {
        dict = new Dictionary<string, string>();
        dict.Add("alpha", "<color=#9c1cbf>");
        dict.Add("beta", "<color=#bf7f1c>");
        dict.Add("gamma", "<color=#1cbf7f>");

        dict.Add("points", "<color=#9e193f>");
        dict.Add("value", "<color=#1c199e>");
        dict.Add("bonus", "<color=#5d196e>");
        dict.Add("chance", "<color=#4f6e19>");

        dict.Add("oros", "<color=#d7ba13>");
        dict.Add("copas", "<color=#f31b1b>");
        dict.Add("espadas", "<color=#68b1d3>");
        dict.Add("bastos", "<color=#497d3e>");

        dict.Add("black", "<color=#000000>");

        fractions = new Dictionary<float, string>();
        fractions.Add(1f / 2, "1/2");
        fractions.Add(1f / 4, "1/4");
        fractions.Add(1f / 7, "1/7");
    }

    public string Color(string text, string colorKey)
    {
        dict.TryGetValue(colorKey, out string colorHex);
        if (colorHex == null)
        {
            return text;
        }
        return colorHex + text + "</color>";
    }

    public string PointsText(int points)
    {
        return dict["points"] + "+" + points.ToString() + " puntos</color>";
    }

    public string ValueText(int value)
    {
        return dict["value"] + "+" + value.ToString() + " valor</color>";
    }

    public string BonusText(int value)
    {
        return dict["bonus"] + "+" + value.ToString() + " bonus</color>";
    }

    public string ChanceText(float chance)
    {
        return dict["chance"] + fractions[chance] + " de probabilidad </color>";
    }
}
