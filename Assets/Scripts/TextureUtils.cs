using UnityEngine;

public static class TextureUtils
{
    public static void SetPixel2D(this Texture2D texture, int x, int y, Color color)
    {
        // Debug.Log($"({x}, {y}) set pixel at position {x}, {texture.height - 1 - y}");
        texture.SetPixel(x, texture.height - 1 - y, color);
    }
}