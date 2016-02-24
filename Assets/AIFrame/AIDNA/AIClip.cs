using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// AI 片断，可以理解为AI状态机中的一个状态片断
/// </summary>
public class AIClip
{
    public string name = "empty";
    /// <summary>
    /// 当前片断可能切换到的片断列表
    /// </summary>
    public List<AILinkClip> linkAIClipList = new List<AILinkClip>();
    /// <summary>
    /// 这个片断应该播放的动画名称
    /// </summary>
    public string animationName="";

    public float attackRange;

}


public class AILinkClip
{
    public AILinkClip()
    {
        
    }
    //!!!!!!!!!!!!!! 注意，用XML序列化的时候如果定义了这个带参数的构造函数，没有无参数的构造函数就会保存
    //导致构造序列化失败
    public AILinkClip(AIClip clip)
    {
        linkToClip = clip;
    }
    public List<AILinkCondiction> linkConditionList = new List<AILinkCondiction>();
    public AIClip linkToClip = new AIClip();
    public bool checkAllCondition;
}


