using System.Collections.Generic;
using UnityEngine;

public class PuntuacionesCartas
{
    public static readonly Dictionary<Valor, int> cardPoints = new()
    {
        { Valor.As, 11 },
        { Valor.Dos, 2 },
        { Valor.Tres, 10 },
        { Valor.Cuatro, 4 },
        { Valor.Cinco, 5 },
        { Valor.Seis, 6 },
        { Valor.Siete, 7 },
        { Valor.Sota, 8 },
        { Valor.Caballo, 9 },
        { Valor.Rey, 9 },
    };

    public static readonly Dictionary<Valor, int> cardWinOrder = new()
    {
        { Valor.As, 1 },
        { Valor.Tres, 2 },
        { Valor.Rey, 3 },
        { Valor.Caballo, 4 },
        { Valor.Sota, 5 },
        { Valor.Siete, 6 },
        { Valor.Seis, 7 },
        { Valor.Cinco, 8 },
        { Valor.Cuatro, 9 },
        { Valor.Dos, 10 },
    };
}
