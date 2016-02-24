using UnityEngine;
using System.Collections;
using UnityEditor;
public class AITestWnd : EditorWindow {
    [MenuItem("AIFrame/Open/TestTool")]
	static void Start ()
    {
        AITestWnd wnd = EditorWindow.GetWindow<AITestWnd>();
    }

    private string aiModelName="";

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        aiModelName = AIFUIUtility.DrawTextField(aiModelName, "AI资源名");
        if (GUILayout.Button("Create"))
        {
            AIMgr.instance.CreateAI(aiModelName);
        }
        GUILayout.EndHorizontal();


    }
}
