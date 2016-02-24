using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
public class AILinkCondictionSelectWnd:EditorWindow
{
    public delegate  void SelectConditon(AILinkCondiction con);

    public SelectConditon onSelect;
    void OnGUI()
    {
        if (GUILayout.Button("变量条件"))
        {
            SelectConditionType(Activator.CreateInstance<AiVarCondiction>());
        }

        if (GUILayout.Button("输入条件"))
        {
            SelectConditionType(Activator.CreateInstance<AiInputCondiction>());
        }
        
        if(GUILayout.Button("目标状态"))
        {
            SelectConditionType(Activator.CreateInstance<AiTargetStateCondiction>());
        }

    }

    void SelectConditionType(AILinkCondiction condiction)
    {
        if (onSelect != null)
        {
            onSelect(condiction);
        }
    }
	
}
