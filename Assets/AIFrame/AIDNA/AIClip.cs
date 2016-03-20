using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// AI 片断，可以理解为AI状态机中的一个状态片断
/// </summary>
[System.Serializable]
public class AIClip
{
    public string NameOnUI
    {
        get { return string.Format(string.Format("{0}_{1}_{2}",clipKey, animationName, name)); }
    }

    public string name = "";
    /// <summary>
    /// 当前片断可能切换到的片断列表
    /// </summary>
    public List<AILink> linkAIClipList = new List<AILink>();
    /// <summary>
    /// 片断键值
    /// </summary>
    public string clipKey = "";
    /// <summary>
    /// 这个片断应该播放的动画名称
    /// </summary>
    public string animationName="";

    public float attackRange;
    /// <summary>
    /// 这个片断会触发的事件列表
    /// </summary>
    public List<AIClipEvent> AiClipEvents = new List<AIClipEvent>();
    /// <summary>
    /// 默认连接（当前动作播完接着要播的）片断 要注意这里连接的是片断键值，不是动画名字
    /// </summary>
    public string defaultLinkClip = "";

    /// <summary>
    /// 是否检测方向输入， 检测的话可能会切换到跑或者走的动画
    /// </summary>
    public bool CheckDirectionInput=false;
    //动画片断时长，秒为单位
    public float animationTime = 1;

    /// <summary>
    /// 向目标靠近
    /// </summary>
    public bool runToTarget=false;

    /// <summary>
    /// 击中定义列表
    /// </summary>
    public List<AiClipHitData> hitCheckList = new List<AiClipHitData>(); 
}

[System.Serializable]
public class AILink
{
    public List<AILinkCondiction> linkConditionList = new List<AILinkCondiction>();
    /// <summary>
    /// 连接的片断键值名
    /// </summary>
    public string linkToClip = "";
    public bool checkAllCondition;

    
}


