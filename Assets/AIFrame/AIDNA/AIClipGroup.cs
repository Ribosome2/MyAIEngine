using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIDataSet
{
    public List<AIDataUnit> aiDataList=new List<AIDataUnit>();
}


/// <summary>
/// 单个英雄的AI数据，一个英雄可以在AI组库里选择一个作为当前AI组， 
/// 比如主控组切换到跟随组等等
/// </summary>
public class AIDataUnit
{
    public string NameOnUI
    {
        get { return Id + AiName; }
    }
    public int Id; //AI数据的唯一标识
    public string AiName="undefined";
    public List<AIClipGroup> aiGroups = new List<AIClipGroup>();
}

/// <summary>
/// AI 片断组，用于保存一个AI所有可能切换的AIClip以及在这个状态组的基本属性信息
/// </summary>
public class AIClipGroup
{
    public string GroupName
    {
        get
        {
            if(string.IsNullOrEmpty(name))
            {
                return "未定义";
            }
            return name;
        }
        set { name = value; }
    }

    public string name="";
    public AIShape shape=new AIShape();
    public List<AIClip> aiClipList = new List<AIClip>();

}


/// <summary>
/// 定义一个AI的外表形态，碰撞体宽高，整体Scale等
/// </summary>
public class AIShape
{
    /// <summary>
    /// 生成的GameObject 本地Scale
    /// </summary>
    public float scaleRatio=1;

    public float colliderHeight=1.8f;
    public float colliderRadius=0.5f;

    public float hitDetectHeight;
    public float hitDetectScale;
    public float hitRadius;

}
