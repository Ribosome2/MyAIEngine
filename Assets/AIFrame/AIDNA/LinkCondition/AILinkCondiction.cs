using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
[System.Serializable]
[XmlInclude(typeof(AiVarCondiction))]
[XmlInclude(typeof(AiInputCondiction))]
[XmlInclude(typeof(AiTargetStateCondiction))]

//!!!!!! 序列化有继承关系的列表的话， 没有XmlInclude 会序列化失败
public abstract class AILinkCondiction
{
#if UNITY_EDITOR
   // public abstract bool IsConditionMet();
    public virtual void OnEditorUI()
    {
       

    }
#endif
}



