using System.Xml.Serialization;
using UnityEngine;
using System.Collections;


/// <summary>
/// Ai片断可以触发的事件
/// </summary>
[XmlInclude(typeof(ShowEffectEvent)),XmlInclude(typeof(PlayAudioEvent))]
public abstract class AIClipEvent
{

    public string eventName = "aiEvent";
    /// <summary>
    /// 触发时间，0表示立刻执行
    /// </summary>
    public float triggerTime=0;
#if UNITY_EDITOR
    public virtual void OnDrawEditor()
    {
        
    }
#endif 
}
