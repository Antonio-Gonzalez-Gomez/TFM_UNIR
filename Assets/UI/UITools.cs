using UnityEngine;

public class UITools
{
    private float maxHeight;
    private float maxWidth;
    private Vector2 screenSize;
    public UITools(Vector2 resolution)
    {
        this.maxHeight = Camera.main.orthographicSize;
        this.maxWidth = maxHeight * Screen.width / Screen.height;
        this.screenSize = resolution;
    }

    public Vector2 WorldPositionToAnchor(Vector3 worldPosition)
    {
        return new Vector2(0.5f * screenSize.x * worldPosition.x / maxWidth,
            0.5f * screenSize.y * worldPosition.y / maxHeight);
    }

    public Vector3 CursorToWorldPosition(Vector2 cursor)
    {
        return new Vector3((cursor.x - 0.5f * screenSize.x) * 2 * maxWidth / screenSize.x,
            (cursor.y - 0.5f * screenSize.y) * 2 * maxHeight / screenSize.y,
            -50f);
    }
}
