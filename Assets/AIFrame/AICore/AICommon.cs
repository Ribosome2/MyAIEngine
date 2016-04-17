﻿/// <summary>
/// AI的阵营
/// </summary>
public enum EAiCamp
{
    /// <summary>
    /// 主角
    /// </summary>
    MainPlayer,
    /// <summary>
    /// 同伴
    /// </summary>
    Friend,
    /// <summary>
    /// 敌方
    /// </summary>
    Enemy,

}

public enum EHitCheckShape
{
    Capsule,//胶囊体
    Cylinder,//圆柱体
    /// <summary>
    /// 扇形
    /// </summary>
    Fan,
    Circle,
    LaserBeam,//类似激光束的攻击定义，可以是任意角度

}



