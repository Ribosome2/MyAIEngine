using UnityEngine;
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

    void OnGUI()
    {
        if (aiLink!=null)
        {
            aiLink.checkAllCondition = GUILayout.Toggle(aiLink.checkAllCondition,"检查所有条件");
            aiLink.linkToClip = AIFUIUtility.DrawTextField(aiLink.linkToClip, "目标片断");
            AIFUIUtility.DrawAiLinkConditions(aiLink);

        }

        GUILayout.Button("确定");
    }

    

}
