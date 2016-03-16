using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class AIDebugWindow : EditorWindow {

    public static void Open()
    {
        EditorWindow.GetWindow<AIDebugWindow>();
    }

    private Vector2 scrollPos;
    private AIUnit mDebugUnit;
    void OnGUI()
    {
        if (mDebugUnit!=null)
        {
             DrawAIState(mDebugUnit);
        }
        GUILayout.BeginArea(new Rect(position.width*0.6f,0,position.width*0.3f,position.height));
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        List<AIUnit> mAiUnits = AIMgr.instance.listAIs;
        for (int i = 0; i < mAiUnits.Count; i++)
        {
            AIUnit ai = mAiUnits[i];
            if (ai != null)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(ai.Name))
                {
                    mDebugUnit = ai;
                }
                if (GUILayout.Button("Delete"))
                {
                    AIMgr.instance.DestroyAllAIs();
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();

    }

    void DrawAIState(AIUnit tarUnit)
    {
        GUILayout.Label(tarUnit.AiGroupData.GroupName);
        GUILayout.Label("动画名"+tarUnit.CurAiClip.animationName);
        GUILayout.Label("默认连接"+tarUnit.CurAiClip.defaultLinkClip);
        GUILayout.Label("Check input"+tarUnit.CurAiClip.CheckDirectionInput);
        GUILayout.Label("Camp:"+tarUnit.aiCamp);
    }

}
