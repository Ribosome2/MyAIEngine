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
public enum AITargetState
{
    /// <summary>
    /// 在视野中
    /// </summary>
    InSight,
    /// <summary>
    /// 在可以攻击范围
    /// </summary>
    InAttackRange,
    OutOfRange,
}
/// <summary>
/// 目标（）改变事件
/// </summary>
public class AiTargetStateCondiction:AILinkCondiction
{
    public ETargetType targetType;
    public AITargetState targetState;



}
