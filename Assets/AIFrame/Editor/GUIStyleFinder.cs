using UnityEditor;
using UnityEngine;
using System.Collections;

public class GUIStyleFinder : EditorWindow {
    [MenuItem("AIFrame/GUIStyleFinder")]
    public static void OpenGUIStyleFinder()
    {
        EditorWindow.GetWindow<GUIStyleFinder>();
    }

    private Vector2 scrollPos;
    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUIStyle[] styles = GUI.skin.customStyles;
        for (int i = 0; i < styles.Length; i++)
        {
            GUIStyle style = styles[i];
            GUILayout.BeginHorizontal();
            GUILayout.Button(style.name, GUI.skin.FindStyle(style.name));
            GUILayout.TextField(style.name,GUILayout.Width(200));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }
}
