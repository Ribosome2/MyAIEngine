using UnityEngine;
using System.Collections;

public enum EAiEventType
{
    OnUpdate,
    SwitchAiClip,
    Dead,
    HpChange,
}

public interface AIEventListener
{
    void OnEvent(AIUnit eventAI,EAiEventType eventType);

}
