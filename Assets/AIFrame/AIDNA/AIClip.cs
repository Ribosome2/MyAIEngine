using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// AI 片断，可以理解为AI状态机中的一个状态片断
/// </summary>
public class AIClip
{
    public string NameOnUI
    {
        get { return string.Format(string.Format("{0}/{1}", animationName, name)); }
    }

    public string name = "";
    /// <summary>
    /// 当前片断可能切换到的片断列表
    /// </summary>
    public List<AILink> linkAIClipList = new List<AILink>();
    /// <summary>
    /// 这个片断应该播放的动画名称
    /// </summary>
    public string animationName="";

    public float attackRange;
    /// <summary>
    /// 这个片断会触发的事件列表
    /// </summary>
    public List<AIClipEvent> mListEvents = new List<AIClipEvent>();
    /// <summary>
    /// 默认连接（当前动作播完接着要播的）片断
    /// </summary>
    public string defaultLinkClip = "";

    /// <summary>
    /// 是否检测方向输入， 检测的话可能会切换到跑或者走的动画
    /// </summary>
    public bool CheckDirectionInput=false;
    //动画片断时长，秒为单位
    public float animationTime = 1; 
}


public class AILink
{
    public List<AILinkCondiction> linkConditionList = new List<AILinkCondiction>();
    public string linkToClip = "";
    public bool checkAllCondition;

    
}


