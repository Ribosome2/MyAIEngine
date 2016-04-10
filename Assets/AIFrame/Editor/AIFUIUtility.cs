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
            content = EditorGUILayout.TextField(content, GUILayout.Width(width));

        }
        else
        {
            content = EditorGUILayout.TextField(content);
        }
        GUILayout.EndHorizontal();
        return content;
    }

    
    public static void  DrawAiLinkPopup(AIClipGroup clipGroup, AILink link)
    {
        if (clipGroup == null)
        {
            GUILayout.Label("Ai组为空");
            return;
        }
        if (clipGroup.aiClipList.Count == 0)
        {
            GUILayout.Label("片断列表为空");
            return ;
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("连接动画名", GUILayout.Width(70));
        string[] optionClips =new string[clipGroup.aiClipList.Count];
        for (int i = 0; i < optionClips.Length; i++)
        {
            optionClips[i] = clipGroup.aiClipList[i].NameOnUI;
        }
        int curSelectIndex = clipGroup.aiClipList.FindIndex(delegate(AIClip targetClip)
        {
            return targetClip.clipKey == link.linkToClip;
        });
        if (curSelectIndex < 0)
            curSelectIndex = 0;

        curSelectIndex = EditorGUILayout.Popup(curSelectIndex, optionClips); //最终的选择
        link.linkToClip = clipGroup.aiClipList[curSelectIndex].clipKey;
     
        GUILayout.EndHorizontal();
    }

    public static string  DrawAiLinkPopup(AIClipGroup clipGroup,  string curSelect,string strTip,int popUpWidth)
    {
        if (clipGroup == null)
        {
            GUILayout.Label("Ai组为空");
            return"";
        }
        if (clipGroup.aiClipList.Count == 0)
        {
            GUILayout.Label("片断列表为空");
            return "";
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label(strTip, GUILayout.Width(70));
        string[] optionClips = new string[clipGroup.aiClipList.Count];
        for (int i = 0; i < optionClips.Length; i++) //显示的是全名， 但是我们记录的是键值
        {
            optionClips[i] = clipGroup.aiClipList[i].NameOnUI;
        }
        int curSelectIndex = clipGroup.aiClipList.FindIndex(delegate(AIClip targetClip)
        {
            return targetClip.clipKey == curSelect;
        });
        if (curSelectIndex < 0)
            curSelectIndex = 0;

        curSelectIndex = EditorGUILayout.Popup(curSelectIndex, optionClips, GUILayout.Width(popUpWidth));
        curSelect = clipGroup.aiClipList[curSelectIndex].clipKey;
        GUILayout.EndHorizontal();

        return curSelect;
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

    public static bool LayoutButtonWithColor(string content, Color color,GUIStyle style,params GUILayoutOption[] layoutOpt)
    {
        Color prevColor = GUI.color;
        GUI.color = color;
        bool click = GUILayout.Button(content,style, layoutOpt);
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
        AIFUIUtility.DrawVector3("碰撞偏移", ref shape.colliderOffset);
        EditorGUILayout.Separator();
        shape.hitDetectScale = EditorGUILayout.FloatField("攻击框比例:", shape.scaleRatio, GUILayout.Width(300));
        shape.hitDetectHeight = EditorGUILayout.FloatField("攻击框高度:", shape.colliderHeight, GUILayout.Width(300));
        shape.hitRadius = EditorGUILayout.FloatField("攻击框半径:", shape.colliderRadius, GUILayout.Width(300));
    }

    public static  void DrawVector3(string name,ref Vector3 v3)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(name, GUILayout.Width(80));
        v3 = EditorGUILayout.Vector3Field("", v3,GUILayout.Width(200));
        GUILayout.EndHorizontal();
    }


    public static void DrawCommanAnimation(AICommonAnimation  comAnim,AIClipGroup clipGroup)
    {
        GUILayout.Label("AI通用片断");
        comAnim.die = AIFUIUtility.DrawAiLinkPopup(clipGroup, comAnim.die, "  默认连接", 150);
        comAnim.hit = AIFUIUtility.DrawAiLinkPopup(clipGroup, comAnim.hit, "  受击", 150);
        comAnim.idle = AIFUIUtility.DrawAiLinkPopup(clipGroup, comAnim.idle, "  默认待机", 150);
        comAnim.run = AIFUIUtility.DrawAiLinkPopup(clipGroup, comAnim.run, "  跑步", 150);
        comAnim.walk = AIFUIUtility.DrawAiLinkPopup(clipGroup, comAnim.walk, "  走路", 150);

    }

    /// <summary>
    /// 绘制片断时间列表
    /// </summary>
    /// <param name="clip"></param>
    public static void DrawAiEvetList(AIClip clip)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("事件列表", GUILayout.Width(60));
        #region copy and paste icons
        if (GUILayout.Button(AIFGUISKin.IconCopy, GUILayout.Width(30), GUILayout.Height(30)))
        {
            if (AIDataSelection.aiEvent != null)
            {
                CustomClipBoard.CopyData(AIDataSelection.aiEvent);
            }
        }
        if (GUILayout.Button(AIFGUISKin.IconPaste, GUILayout.Width(30), GUILayout.Height(30)))
        {
            object clipBoardObj;
            CustomClipBoard.GetCopyObject(out clipBoardObj);
            if (clipBoardObj == null)
            {
                Debug.LogError("剪切板没有内容可以粘贴");
            }
            else
            {
                clipBoardObj = Utility.XmlDeepCloneObject(clipBoardObj);

                if (clipBoardObj is AIClipEvent)
                {
                    clip.AiClipEvents.Add(clipBoardObj as AIClipEvent);
                }

            }
        }
        #endregion
        if (GUILayout.Button(AIFGUISKin.IconNewItem, GUILayout.Width(30),GUILayout.Height(30)))
        {
            AIEventEditWnd.OpenCreateEditor(delegate(AIClipEvent e)
            {
                clip.AiClipEvents.Add(e);
            });
        }
        GUILayout.EndHorizontal();
        if (clip.AiClipEvents.Count == 0)
        {
            GUILayout.Label("事件列表为空");
        }
        else
        {
            for (int i = 0; i < clip.AiClipEvents.Count; i++)
            {
                AIClipEvent evet = clip.AiClipEvents[i];
                GUILayout.BeginHorizontal();
                Color col = AIDataSelection.aiEvent == evet ? Color.green : GUI.color;
                if (AIFUIUtility.LayoutButtonWithColor(evet.eventName, col, 150)) 
                {
                    AIDataSelection.SelectedAIEvent(evet);
                    AIEventEditWnd.OpenAsEditMode(evet);
                }
                if(GUILayout.Button("X",GUILayout.Width(30)))
                {
                    if (EditorUtility.DisplayDialog("警告", "确定删除事件吗", "确定", "取消"))
                    {
                        clip.AiClipEvents.Remove(evet);
                        return;
                    }

                }
                GUILayout.EndHorizontal();
            }
        }
       
    }

    /// <summary>
    /// 绘制攻击定义列表
    /// </summary>
    /// <param name="clip"></param>
    public static void DrawHitDefinitionList(AIClip clip)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("攻击定义列表:",GUILayout.Width(80));
        #region copy and paste icons
        if (GUILayout.Button(AIFGUISKin.IconCopy, GUILayout.Width(30), GUILayout.Height(30)))
        {
            if (AIDataSelection.selectedHitData != null)
            {
                CustomClipBoard.CopyData(AIDataSelection.selectedHitData);
            }
        }
        if (GUILayout.Button(AIFGUISKin.IconPaste, GUILayout.Width(30), GUILayout.Height(30)))
        {
            object clipBoardObj;
            CustomClipBoard.GetCopyObject(out clipBoardObj);
            if (clipBoardObj == null)
            {
                Debug.LogError("剪切板没有内容可以粘贴");
            }
            else
            {
                clipBoardObj = Utility.XmlDeepCloneObject(clipBoardObj);

                if (clipBoardObj is AiClipHitData)
                {
                    clip.hitCheckList.Add(clipBoardObj as AiClipHitData);
                }

            }
        }
        #endregion
        if(GUILayout.Button(AIFGUISKin.IconNewItem,GUILayout.Width(30),GUILayout.Height(30)))
        {
            clip.hitCheckList.Add(new AiClipHitData());
        }

       
        GUILayout.EndHorizontal();
        for (int i = 0; i < clip.hitCheckList.Count; i++)
        {
            AiClipHitData hitCheck = clip.hitCheckList[i];
            GUILayout.BeginHorizontal();
            Color col = hitCheck == AIDataSelection.selectedHitData?Color.green:GUI.color;
            if(AIFUIUtility.LayoutButtonWithColor(hitCheck.name,col,150))
            {
                AIDataSelection.SelectedHitData(hitCheck);
                HitDataEditWnd.Open(hitCheck);
            }
            if(GUILayout.Button("X",GUILayout.Width(40)))
            {
                if (EditorUtility.DisplayDialog("tips", "确定删除攻击定义？", "OK"))
                {
                    clip.hitCheckList.Remove(hitCheck);
                    break;
                    
                }
            }
            GUILayout.EndHorizontal();
        }
    }

    #region DrawLinkCondition
    public static void DrawAiLinkConditions(AILink link)
    {
        List<AILinkCondiction> conditionList = link.linkConditionList;
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
            link.checkAllCondition = GUILayout.Toggle(link.checkAllCondition, "检查全部条件", GUILayout.Width(100));
            foreach (AILinkCondiction condition in conditionList)
            {
               
               // GUILayout.Label("__________________");
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
                EditorGUILayout.Separator();

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
    public static void DrawLinkConditon(AiTargetStateCondiction con)
    {
        con.targetType = (ETargetType)DrawCustomEnum("目标类型", con.targetType, 50);
        con.targetState= (ETargetState)DrawCustomEnum("目标状态", con.targetState, 50);
        con.targetDistance = EditorGUILayout.FloatField("目标距离", con.targetDistance, GUILayout.ExpandWidth(false));
    } 
    #endregion

    public static void DrawAIClip(AIClip clip,AIClipGroup paretGroup)
    {
        clip.clipKey = AIFUIUtility.DrawTextField(clip.clipKey, "动画片断键值", 100);
        clip.name = AIFUIUtility.DrawTextField(clip.name, "片断名称", 200);
        clip.animationName = AIFUIUtility.DrawTextField(clip.animationName, "动画名称", 200);
        if (paretGroup != null)
        {
            clip.defaultLinkClip = AIFUIUtility.DrawAiLinkPopup(paretGroup, clip.defaultLinkClip, "  默认连接", 150);
        }
        clip.animationTime = EditorGUILayout.FloatField("动画时长", clip.animationTime, GUILayout.Width(120), GUILayout.ExpandWidth(true));
        clip.attackRange = EditorGUILayout.FloatField("攻击范围",clip.attackRange, GUILayout.Width(90));
        clip.CheckDirectionInput = GUILayout.Toggle(clip.CheckDirectionInput, "方向输入", GUILayout.Width(90));
        clip.runToTarget = GUILayout.Toggle(clip.runToTarget, "跑向目标", GUILayout.Width(90));
        clip.applyRootMotion = GUILayout.Toggle(clip.applyRootMotion, "使用动画运动", GUILayout.Width(90));

    }


}
