using UnityEngine;

public class RNG
{

    public static bool RandomRange(float chance)
    {
        float randomF = Random.Range(0f, 1f);
        return chance > randomF;
    } 
}
