using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum EAiActionType
{
    FaceTarget, //触发朝向当前目标一次目标

}
/// <summary>
/// 简单的执行某个动作的事件，只有一个事件类型，不用什么参数，所以以后普通的事件放这里
/// </summary>
public class AiActionEvent:AIClipEvent
{
    public EAiActionType actiomType;

}

