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
    public EAiCamp createCamp;
    private bool createAsAI;

    void OnGUI()
    {

        if(GUILayout.Button("ClearAllAI",GUILayout.Width(100)))
        {
            AIMgr.instance.DestroyAllAIs();
        }
        if (GUILayout.Button("DebugWindow", GUILayout.Width(100)))
        {
            AIDebugWindow.Open();
        }
        createCamp = (EAiCamp) EditorGUILayout.EnumPopup("创建阵营", createCamp);
        createAsAI = EditorGUILayout.Toggle("asAi", createAsAI);
        GUILayout.BeginHorizontal();
        aiModelName = AIFUIUtility.DrawTextField(aiModelName, "AI资源名");
        aiDataId = EditorGUILayout.IntField("AI数据ID", aiDataId);
       
        if (GUILayout.Button("Create"))
        {
            AIUnit ai= AIMgr.instance.CreateAI(aiModelName,aiDataId);
            ai.aiCamp = createCamp;
            ai.SwitchAI(createAsAI);
        }
        GUILayout.EndHorizontal();


    }
}
