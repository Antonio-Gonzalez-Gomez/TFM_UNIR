using System.Collections.Generic;
using UnityEngine;

public class DeckShuffler
{
    //Algoritmo de Fisher-Yates (permutaciones equiprobables de una lista)
    //Se usa para barajar el mazo tras ser creado (o tras cada ronda)
    public static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
