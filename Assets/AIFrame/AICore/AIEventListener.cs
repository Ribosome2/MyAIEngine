using UnityEngine;
using System.Collections;

public enum EAiEventType
{
    SwitchAiClip,
    Dead,
    HpChange,
}

public interface AIEventListener
{
    void OnEvent(AIUnit eventAI,EAiEventType eventType);
    void OnUpdateAI(float deltaTime);

}
