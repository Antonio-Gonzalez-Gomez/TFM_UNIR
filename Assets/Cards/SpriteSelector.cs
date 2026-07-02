using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    //Singleton para manejar el cambio dinamico del sprite de las cartas
    public static SpriteSelector Instance { get; private set; }
    private Sprite[] cardSprites;

    private void Awake()
    {
        Instance = this;
        cardSprites = Resources.LoadAll<Sprite>("Sprites/baraja");
    }

    public static Sprite GetSpriteByIndex(int index)
    {
        return Instance.cardSprites[index];
    }
}
