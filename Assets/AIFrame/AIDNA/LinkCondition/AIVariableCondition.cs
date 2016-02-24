using UnityEngine;
using System.Collections;

public enum CompareType
{
    Larger,
    Equal,
    Small
}

/// <summary>
/// 变量类型
/// </summary>
public enum VarType
{
    Hp,
    Mp,
}

/// <summary>
/// 检测变量条件
/// </summary>
public class AiVarCondiction :AILinkCondiction
{
    public VarType varType=VarType.Hp;
    public CompareType compareType=CompareType.Equal;

}
