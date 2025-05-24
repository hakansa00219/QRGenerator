using UnityEngine;

public static class TextureUtils
{
    public static void SetPixel2D(this Texture2D texture, int x, int y, Color color)
    {
        texture.SetPixel(x, texture.height - 1 - y, color);
    }
    public static void SetPixel2D(this Texture2D texture, int x, int y, bool value)
    {
        texture.SetPixel(x, texture.height - 1 - y, value ? Color.black : Color.white);
    }
    public static void SetPixel2D(this Texture2D texture, int x, int y, byte data, int index)
    {
        texture.SetPixel(x, texture.height - 1 - y, ((data >> index) & 1) == 1 ? Color.black : Color.white);
    }
    public static void SetPixel2D(this Texture2D texture, int x, int y, int data, int index)
    {
        texture.SetPixel(x, texture.height - 1 - y, ((data >> index) & 1) == 1 ? Color.black : Color.white);
    }
    public static Color GetPixel2D(this Texture2D texture, int x, int y)
    {
        return texture.GetPixel(x, texture.height - 1 - y);
    }
}