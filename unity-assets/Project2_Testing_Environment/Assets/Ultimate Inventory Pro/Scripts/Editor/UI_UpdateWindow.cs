using UnityEngine;
using UnityEditor;
using System.Collections;

public class UI_UpdateWindow : EditorWindow
{

    Texture2D banner;
    public static string winText;

    void OnEnable()
    {
        banner = Resources.Load("banner") as Texture2D;
    }

    public static void ShowWindow(string txt)
    {
        EditorWindow a = GetWindow(typeof(UI_UpdateWindow), true, "Ultimate Inventory 5 - Update", true) as EditorWindow;
        a.minSize = new Vector2(370, 360);
        a.maxSize = a.minSize;
        winText = txt;
    }
   
    void OnGUI()
    {
        GUILayout.Box(banner,GUILayout.Width(350),GUILayout.Height(225));
        GUILayout.Label("");
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label(winText);
        GUILayout.BeginHorizontal();
        if (!winText.Contains("latest"))
        {
            if (GUILayout.Button("Open In Store", GUILayout.Width(150), GUILayout.Height(25)))
            {
                try
                {
                    System.Diagnostics.Process.Start("https://www.assetstore.unity3d.com/#!/content/19900");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("System failed to perform : \"Open In Store\" action!");
                    Debug.LogException(ex);
                }
            }
        }
        if (GUILayout.Button("Close", GUILayout.Width(150), GUILayout.Height(25)))
        {
            Close();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

}
