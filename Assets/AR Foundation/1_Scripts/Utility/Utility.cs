using System;
using System.IO;
using UnityEngine;

public class Utility 
{

    public static float PixalToMeterValue = 0.0002645833f;
    public static float InchToMeterValue = 0.0254f;

    // incase required to save data locally 
    static string fileBasePath = Application.persistentDataPath;

    public static Sprite Base64StringToSprite(byte[] data) {
        string base64str = Convert.ToBase64String(data);
        Debug.Log(base64str);
        byte[] imageBytes = Convert.FromBase64String(base64str);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        return sprite;
    }


    public static Sprite Texture2DToSprite(Texture2D tex) {

        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        return sprite;

    }

    public  static PaintingInfo Base64ToTexture2D(byte[] data, double width, double height) {

        string encodedData = Convert.ToBase64String(data);
        byte[] imageData = Convert.FromBase64String(encodedData);
        Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false, true);
        texture.LoadImage(imageData);
        Debug.Log("<color=yellow>"+texture.width + "  :  " + texture.height+"</color>");
        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.filterMode = FilterMode.Point;
        texture.LoadImage(imageData);

        PaintingInfo info = new PaintingInfo();
        info.texture = texture;
        info.widht = (float)(width * InchToMeterValue);
        info.height = (float)(height * InchToMeterValue);

        return info;
    }


    public static Sprite TextureToSprite(Texture2D texture) {
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        return sprite;
    }


    public static float PixalToMeter(int pixal) {
        float meter = pixal * PixalToMeterValue;
        return meter;
    }

}
