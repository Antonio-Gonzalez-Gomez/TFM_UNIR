using System.Collections.Generic;
using UnityEngine;

public class CanteDicts
{
    public static readonly Dictionary<Cante, int> valores = new()
    {
        { Cante.Ninguno, 1 },
        { Cante.Infanteria, 2 },
        { Cante.LasVeinte, 2 },
        { Cante.LasCuarenta, 3 },
        { Cante.TutePartido, 4 },
        { Cante.Socare, 5 },
        { Cante.SocareReal, 6 },
        { Cante.Tute, 7 },
    };

    public static readonly Dictionary<Cante, int> bonus = new()
    {
        { Cante.Ninguno, 0 },
        { Cante.Infanteria, 10 },
        { Cante.LasVeinte, 20 },
        { Cante.LasCuarenta, 40 },
        { Cante.TutePartido, 60 },
        { Cante.Socare, 80 },
        { Cante.SocareReal, 100 },
        { Cante.Tute, 120 },
    };

    public static readonly Dictionary<Cante, string> text = new()
    {
        { Cante.Ninguno, "Sin cante" },
        { Cante.Infanteria, "Infantería" },
        { Cante.LasVeinte, "Las Veinte" },
        { Cante.LasCuarenta, "Las Cuarenta" },
        { Cante.TutePartido, "Tute Partido" },
        { Cante.Socare, "Socare" },
        { Cante.SocareReal, "Socare Real" },
        { Cante.Tute, "Tute" },
    };
}
