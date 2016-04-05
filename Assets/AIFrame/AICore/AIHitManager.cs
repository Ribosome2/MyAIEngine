using System.Collections.Generic;
using UnityEngine;
using System.Collections;
/// <summary>
/// 负责管理跟攻击相关的逻辑
/// </summary>
public class AIHitManager:AIEventListener {
    public AIHitManager(AIUnit owener)
    {
        mOwner = owener;
        mOwner.AddEventListener(this);
    }

   private AIUnit mOwner;
   List<AiClipHitData> hitList=new List<AiClipHitData>();
   List<AIHitUnit> hitUnits=new List<AIHitUnit>(); 
   public void CheckHitList()
   {
       float mCurClipTime = mOwner.CurClipTime;
       for (int i = 0; i < hitList.Count; i++)
       {
           AiClipHitData hitData = hitList[i];
           //只计算在攻击时间内的
           if (mCurClipTime >= hitData.startTime && mCurClipTime <= hitData.startTime + hitData.lastTime)
           {
               AIHitUnit hitUnit=new AIHitUnit();
               hitUnit.Init(hitData,mOwner);
               hitUnits.Add(hitUnit);
               hitList.Remove(hitData);

           }
       }
   }

    public void Update()
    {
        CheckHitList();
        UpdateHitUnits();
    }

    void UpdateHitUnits()
    {
        for (int i = 0; i < hitUnits.Count; i++)
        {
            AIHitUnit hitUnit = hitUnits[i];
            if (hitUnit.ShouldDie)
            {
                hitUnit.Destroy();
                hitUnits.Remove(hitUnit);
                break;
            }
            else
            {
                hitUnit.OnUpdate(Time.deltaTime);
            }
        }
    }


   public void Reset()
   {
       hitList.Clear();
       hitList.AddRange(mOwner.CurAiClip.hitCheckList);
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


