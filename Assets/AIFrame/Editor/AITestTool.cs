using System.IO;
using System.Runtime.Remoting.Services;
using UnityEngine;
using System.Collections;
using UnityEditor;
public class AITestWnd : EditorWindow {
    [MenuItem("AIFrame/Open/TestTool #&g")]
	static void Start ()
    {
        AITestWnd wnd = EditorWindow.GetWindow<AITestWnd>();
        LoadAiTable();
    }

    private static void LoadAiTable()
    {
        byte[] bytes = File.ReadAllBytes(Application.dataPath + "/AIFrame/Resources/roleInfo.kiss");
        roleInfoTableManager.instance.LoadData(bytes);
    }

    private string aiModelName="Knight100";
    private int aiDataId =1000;
    public EAiCamp createCamp;
    private bool createAsAI;
    private Vector2 scrollPos;

    void OnGUI() 
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load File New"))
        {
            AIDataMgr.instance.LoadNewData();
        }
        if (GUILayout.Button("重新加载AI配置表"))
        {
            LoadAiTable();
        }
        if(GUILayout.Button("ClearAllAI",GUILayout.Width(100)))
        {
            AIMgr.instance.DestroyAllAIs();
        }
        if (GUILayout.Button("DebugWindow", GUILayout.Width(100)))
        {
            AIDebugWindow.Open();
        }

        GUILayout.EndHorizontal();

       
        createCamp = (EAiCamp) EditorGUILayout.EnumPopup("创建阵营", createCamp);
        createAsAI = EditorGUILayout.Toggle("asAi", createAsAI);
        GUILayout.BeginHorizontal();
        aiModelName = AIFUIUtility.DrawTextField(aiModelName, "AI资源名");
        aiDataId = EditorGUILayout.IntField("AI数据ID", aiDataId);
        GUILayout.EndHorizontal();
         GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create"))
        {
            CreateAI(aiModelName,aiDataId,createCamp,createAsAI);
        }

        if (GUILayout.Button("CreateEnemy"))
        {
            CreateAI(aiModelName, aiDataId, EAiCamp.Enemy, true);
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("创建列表");
        GUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < roleInfoTableManager.instance.Size(); i++)
        {
            roleInfo info = roleInfoTableManager.instance.GetByIndex(i);
            GUILayout.BeginHorizontal();
            GUILayout.Label(info.ID + "/" + info.AiDataId + "/" + info.name,GUILayout.Width(200));
            if (GUILayout.Button("创建为主角"))
            {
                CreateAI(info.resModel, info.AiDataId, EAiCamp.MainPlayer, false);
            }
            if (GUILayout.Button("创建为友方"))
            {
                CreateAI(info.resModel, info.AiDataId, EAiCamp.Friend, true);
            }
            if (GUILayout.Button("创建为敌方"))
            {
                CreateAI(info.resModel, info.AiDataId, EAiCamp.Enemy, true);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();


    }

    void CreateAI(string modelName, int aiId, EAiCamp camp, bool asAI)
    {
        AIUnit ai = AIMgr.instance.CreateAI(modelName, aiId);
        ai.aiCamp = camp;
        ai.SwitchAI(asAI);
    }


}
