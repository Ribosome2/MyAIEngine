using System.Collections.Generic;
using UnityEngine;
using System.Collections;
/// <summary>
/// 负责管理AI片断的事件的逻辑
/// </summary>
public class AIEventController:AIEventListener {
    public AIEventController(AIUnit owener)
    {
        mOwner = owener;
        mOwner.AddEventListener(this);
    }

   private AIUnit mOwner;
   List<AIClipEvent> eventList=new List<AIClipEvent>();


    public void Update()
    {

        for (int i = 0; i < eventList.Count; i++)
        {
            AIClipEvent clipEvent = eventList[i];
            if ( mOwner.CurClipTime>clipEvent.triggerTime )
            {
                TriggerEvent(clipEvent);
                eventList.Remove(clipEvent);
            }
        }
    }

    void TriggerEvent(AIClipEvent clipEvent)
    {
        if (clipEvent is SetVelocityEvent)
        {
            SetVelocityEvent velocityEvent = clipEvent as SetVelocityEvent;
            mOwner.SetExtrenalVelocity(velocityEvent.velocity);
        }
        else
        {
            Debug.LogError("未实现的事件类型"+clipEvent);
        }
    }

   public void Reset()
   {
       eventList.Clear();
       eventList.AddRange(mOwner.CurAiClip.AiClipEvents);
   }

   public void OnDead()
   {
       throw new System.NotImplementedException();
   }

   public void OnEvent(AIUnit unit, EAiEventType eventType)
   {
       if (unit != mOwner)
       {
           return;
       }
       switch (eventType)
       {
           case EAiEventType.SwitchAiClip:
           {
              Reset();
              break;
           }
       }
   }





   public void OnUpdateAI(float deltaTime)
   {
       Update();
   }
}


