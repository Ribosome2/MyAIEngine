
using UnityEditor;
using UnityEngine;
using System.Collections;

/// <summary>
/// 目标类型
/// </summary>
public enum ETargetType
{
    /// <summary>
    /// 主角
    /// </summary>
    MainPlayer,
    /// <summary>
    /// 任何敌人
    /// </summary>
    Enemy,
}

/// <summary>
/// 目标状态
/// </summary>
public enum ETargetState
{
   
    /// <summary>
    /// 在可以攻击范围
    /// </summary>
    InAttackRange,
    OutOfRange,
    /// <summary>
    /// 没有找到目标
    /// </summary>
    Lost,
    /// <summary>
    /// 在视野中 暂时就只是意味这可以找到
    /// </summary>
    InSight,
}
/// <summary>
/// 目标（）改变事件
/// </summary>
public class AiTargetStateCondiction:AILinkCondiction
{
    public ETargetType targetType;
    public ETargetState targetState;

    public float targetDistance = 1;

#if UNITY_EDITOR
    public override void OnEditorUI()
    {
        base.OnEditorUI();
        targetType = (ETargetType)EditorGUILayout.EnumPopup("目标类型", targetType);

       targetState = (ETargetState)EditorGUILayout.EnumPopup("目标状态", targetState);
       targetDistance = EditorGUILayout.FloatField("目标距离", targetDistance, GUILayout.ExpandWidth(false));
    }
#endif
}
