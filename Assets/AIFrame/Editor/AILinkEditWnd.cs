﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class AILinkEditWnd :EditorWindow
{
    private AIClipGroup srcGroup;
    private List<AIClip> mOptionList = new List<AIClip>();
    private Vector2 scrollPos;

    public AILink aiLink;

    public void SetData(AIClipGroup clipGroup, AILink link)
    {
        srcGroup = clipGroup;
        aiLink = link;

    }

    public static void EditAILink(AIClipGroup clipGroup, AILink link)
    {
        AILinkEditWnd wnd = EditorWindow.GetWindow<AILinkEditWnd>();
        wnd.SetData(clipGroup,link);
    }
    void OnGUI()
    {
        if (aiLink!=null)
        {
            aiLink.linkToClip = AIFUIUtility.DrawAiLinkPopup(srcGroup, aiLink.linkToClip, "目标片断", 150);
           
            aiLink.checkAllCondition = GUILayout.Toggle(aiLink.checkAllCondition,"检查所有条件");
            aiLink.crossFadeTime = EditorGUILayout.FloatField("动画过度时间", aiLink.crossFadeTime);
           
            GUILayout.BeginHorizontal();
            GUILayout.Label("连接条件：",GUILayout.Width(150));

            if (GUILayout.Button("添加条件",GUILayout.Width(100)))
            {
                AILinkCondictionSelectWnd.SelectNewCondition(delegate(AILinkCondiction con)
                {
                    aiLink.linkConditionList.Add(con);
                });
            }
            GUILayout.EndHorizontal();


            for (int i = 0; i < aiLink.linkConditionList.Count; i++)
            {
                AILinkCondiction con = aiLink.linkConditionList[i];
                EditorGUILayout.Separator();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("X", GUILayout.Width(30)))
                {
                    if (EditorUtility.DisplayDialog("提示", "删除条件？", "OK"))
                    {
                        aiLink.linkConditionList.Remove(con);
                    }
                }
                GUILayout.Label(con.ToString());
                
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();

                con.OnEditorUI();
                GUILayout.EndVertical();
            }

        }

        //GUILayout.Button("确定");
    }

    

}
