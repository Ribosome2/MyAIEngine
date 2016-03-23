using UnityEngine;
using System.Collections;

/// <summary>
/// AI片断的攻击定义
/// </summary>
public class AiClipHitData
{
    public string name = "hit";
    public float startTime = 0;
    public float lastTime=1f;
    public float hitInterval = 0.2f;
    /// <summary>
    /// 显示实体资源名
    /// </summary>
    public string entityResName = "";

    public Vector3 startPosition;
    public HitCheckBase hitCheckData=new HitCheckBase();
    
}


/// <summary>
/// 击中判断基类
/// </summary>
public class HitCheckBase
{
    public EHitCheckShape shapeType = EHitCheckShape.Capsule;
    public Vector3 posOffset = Vector3.zero;
    public float radius = 0.5f;
    public float height = 0;
    
    
}
