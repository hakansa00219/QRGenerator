using System;
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
    public static void SetPixel2D(this Texture2D texture, int x, int y, int blockWidth, int blockHeight, bool value)
    {
        Color[] colorArray = new Color[blockHeight * blockWidth];
        Array.Fill(colorArray, value ? Color.black : Color.white);
        
        texture.SetPixels(x, y, blockWidth, blockHeight, colorArray);
    }
    public static void SetPixel2D(this Texture2D texture, int x, int y, int data, int index)
    {
        texture.SetPixel(x, texture.height - 1 - y, ((data >> index) & 1) == 1 ? Color.black : Color.white);
    }
    public static Color GetPixel2D(this Texture2D texture, int x, int y)
    {
        return texture.GetPixel(x, texture.height - 1 - y);
    }
    public static bool[,] ConvertTo2DArray(this Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        int width = texture.width;
        int height = texture.height;

        bool[,] result = new bool[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                Color color = pixels[index];
                result[x, y] = color == Color.black;
            }
        }

        return result;
    }
}