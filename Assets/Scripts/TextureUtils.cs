using UnityEngine;

public static class TextureUtils
{
    public static void SetPixel2D(this Texture2D texture, int x, int y, Color color)
    {
        texture.SetPixel(x, texture.height - 1 - y, color);
    }
    public static Color GetPixel2D(this Texture2D texture, int x, int y)
    {
        return texture.GetPixel(x, texture.height - 1 - y);
    }
}