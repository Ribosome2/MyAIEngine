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
    private int aiDataId = 0;

    void OnGUI()
    {

        if(GUILayout.Button("ClearAllAI",GUILayout.Width(100)))
        {
            AIMgr.instance.DestroyAllAIs();
        }
        GUILayout.BeginHorizontal();
        aiModelName = AIFUIUtility.DrawTextField(aiModelName, "AI资源名");
        aiDataId = EditorGUILayout.IntField("AI数据ID", aiDataId);
        if (GUILayout.Button("Create"))
        {
            AIMgr.instance.CreateAI(aiModelName,aiDataId);
        }
        GUILayout.EndHorizontal();


    }
}
