using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public Valor valor;
    public Palo palo;
    public int puntos;

    public CardData(Valor valor, Palo palo, int puntos)
    {
        this.valor = valor;
        this.palo = palo;
        this.puntos = puntos;
    }

    public override string ToString()
    {
        return valor + " de " + palo + " (+" + puntos.ToString() + " puntos)";
    }
}
