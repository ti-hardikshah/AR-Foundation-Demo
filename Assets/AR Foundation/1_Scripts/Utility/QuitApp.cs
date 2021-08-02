using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApp : MonoBehaviour
{
    void OnGUI()
    {
        GUIStyle style = new GUIStyle("button");
        style.fontSize = 30;
       

        if (GUI.Button(new Rect(10, 400, 400, 100), "Unload", style)) Application.Unload();
        if (GUI.Button(new Rect(440, 400, 400, 100), "Quit", style)) Application.Quit();
    }
}
