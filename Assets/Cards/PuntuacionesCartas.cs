using System.Collections.Generic;
using UnityEngine;

public class PuntuacionesCartas
{
    public static readonly Dictionary<Valor, int> dict = new()
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
}
