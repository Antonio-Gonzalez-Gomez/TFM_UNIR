using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] Vector2 velocity = new Vector2(-0.2f, 0.2f);
    [SerializeField] float spriteWidth = 2.6f;

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        if (transform.position.x <= -spriteWidth)
            transform.position = new Vector3(0, 0, 50);
    }
}
