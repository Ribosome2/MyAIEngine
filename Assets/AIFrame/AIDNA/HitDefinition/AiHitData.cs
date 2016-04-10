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
    public Vector3 startDirection=new Vector3(0,0,1);
    public float moveSpeed = 0;
    /// <summary>
    /// 是否自动面向攻击目标
    /// </summary>
    public bool autoFaceTarget = false;//
    public HitCheckBase hitCheckData=new HitCheckBase();
   

}


/// <summary>
/// 击中判断基类
/// </summary>
public class HitCheckBase
{
    public EHitCheckShape shapeType = EHitCheckShape.Capsule;
    public Vector3 posOffset = Vector3.zero;
    public float radius = 1.5f;
    public float height = 0.5f;
    /// <summary>
    /// 角度， 一般只有扇形会用到
    /// </summary>
    public float angle = 60;


}
