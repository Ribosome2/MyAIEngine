using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class AIFUIUtility {

    /// <summary>
    /// 绘制一个文本输入框，可以在文本框前面加提示语。自带的似乎没有，所以自己包一层
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string DrawTextField(string content,string strTip,int width=0)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(strTip,GUILayout.ExpandWidth(false));
        if (width > 0)
        {
            content = GUILayout.TextField(content,GUILayout.Width(width));

        }
        else
        {
            content = GUILayout.TextField(content);
        }
        GUILayout.EndHorizontal();
        return content;
    }

    /// <summary>
    /// 用制定颜色画按钮， 画完回复原来的颜色
    /// </summary>
    /// <param name="content"></param>
    /// <param name="color"></param>
    public static bool LayoutButtonWithColor(string content, Color color,int width)
    {
        Color prevColor = GUI.color;
        GUI.color = color;
        bool click=GUILayout.Button(content, GUILayout.Width(width));
        GUI.color = prevColor;
        return click;
    }

    public static Enum DrawCustomEnum(string strTip, Enum enumType, int tipWidth)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(strTip, GUILayout.Width(tipWidth));
        Enum select = EditorGUILayout.EnumPopup(enumType, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
        return select;
    }
    public static void DrawAIShape(AIShape shape)
    {
        GUILayout.Label("AI形态定义");
        shape.scaleRatio = EditorGUILayout.FloatField("缩放比例:" , shape.scaleRatio,GUILayout.Width(300));
        shape.colliderHeight = EditorGUILayout.FloatField("碰撞高度:", shape.colliderHeight, GUILayout.Width(300));
        shape.colliderRadius = EditorGUILayout.FloatField("碰撞半径:", shape.colliderRadius, GUILayout.Width(300));
    }


    #region DrawLinkCondition
    public static void DrawAiLinkConditions(AILinkClip linkClip)
    {
        List<AILinkCondiction> conditionList = linkClip.linkConditionList;
        GUILayout.Label("连接条件:", GUILayout.Width(200));
        if (GUILayout.Button("添加条件", GUILayout.Width(80)))
        {
            AILinkCondictionSelectWnd wnd = EditorWindow.GetWindow<AILinkCondictionSelectWnd>();
            wnd.onSelect = delegate(AILinkCondiction con)
            {
                conditionList.Add(con);
                EditorWindow.GetWindow<AIDataEditor>().Repaint();
            };
        }
        if (conditionList.Count == 0) //
        {
            GUILayout.Label("条件列表为空");
        }
        else
        {
            linkClip.checkAllCondition = GUILayout.Toggle(linkClip.checkAllCondition, "检查全部条件", GUILayout.Width(100));
            foreach (AILinkCondiction condition in conditionList)
            {
                GUILayout.Label("__________________");
                if (condition is AiInputCondiction) //这个基类怎么转成子类？
                {
                    DrawLinkConditon(condition as AiInputCondiction);
                }
                else if (condition is AiVarCondiction)
                {
                    DrawLinkConditon(condition as AiVarCondiction);
                }else if (condition is AiTargetStateCondiction)
                {
                    DrawLinkConditon(condition as AiTargetStateCondiction);
                    
                }
                else
                {
                    Debug.LogError("未实现的变量类型" + condition);
                }


            }
        }
    }


    static void DrawLinkConditon(AiInputCondiction con)
    {
        con.needDiretionInput = GUILayout.Toggle(con.needDiretionInput, "要求方向输入", GUILayout.Width(150));
    }
    static void DrawLinkConditon(AiVarCondiction con)
    {
        con.varType = (VarType)DrawCustomEnum("变量类型", con.varType,70);
        con.compareType = (CompareType)DrawCustomEnum("比较类型", con.compareType,70);
    }
    static void DrawLinkConditon(AiTargetStateCondiction con)
    {
        con.targetType = (ETargetType)DrawCustomEnum("目标类型", con.targetType, 50);
        con.targetState= (AITargetState)DrawCustomEnum("目标状态", con.targetState, 50);
    } 
    #endregion

}
