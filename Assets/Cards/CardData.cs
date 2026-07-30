using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public Valor Valor { get; set; }
    public Palo Palo { get; set; }
    public int Puntos { get; set; }

    public string StringNombre()
    {
        //As de Oros
        return Valor + " de " + Palo;
    }

    public string StringPuntos()
    {
        //+ 11 puntos
        return "+" + Puntos.ToString() + " puntos";
    }
    public override string ToString()
    {
        //As de Oros (+ 11 puntos)
        return StringNombre() + " (" + StringPuntos() + ")";
    }

}
