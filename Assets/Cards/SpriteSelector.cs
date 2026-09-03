using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    //Singleton para manejar el cambio dinamico del sprite de las cartas
    public static SpriteSelector Instance { get; private set; }
    private Sprite[] cardSprites;
    private Sprite[] modSprites;

    private void Awake()
    {
        Instance = this;
        cardSprites = Resources.LoadAll<Sprite>("Sprites/baraja");
        modSprites = Resources.LoadAll<Sprite>("Sprites/modificadores");
    }

    public static Sprite GetSpriteByIndex(int index)
    {
        return Instance.cardSprites[index];
    }
    public static Sprite GetModSpriteByIndex(int index)
    {
        return Instance.modSprites[index];
    }
}
