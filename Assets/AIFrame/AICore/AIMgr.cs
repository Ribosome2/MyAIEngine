using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class AIMgr :Singleton<AIMgr>
{
    public static bool DisableAiAttack = false;
    public Action DrawGizmosEvent;
    public List<AIUnit> listAIs = new List<AIUnit>();

    public AIUnit CreateAI(string resName,int dataId)
    {
        Object obj = Resources.Load(resName);
        if (obj != null)
        {
            GameObject aiModel = Object.Instantiate(obj) as GameObject;
            AIUnit ai = new AIUnit();
            ai.SetModel(aiModel);
            ai.SetAIDataUnit(AIDataMgr.instance.GetAIUnitData(dataId));
            listAIs.Add(ai);
            return ai;
        }
        else
        {
            Debug.LogError(string.Format("资源{0}不存在",resName));
            return null;
        }
    }

    public void OnUpdate()
    {
        InputManager.UpdateInput();

        for (int i = 0; i < listAIs.Count; i++)
        {
            listAIs[i].OnUpdate(Time.deltaTime);
        }
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < listAIs.Count; i++)
        {
            listAIs[i].OnDrawGizmos();
        }

        if (DrawGizmosEvent != null)
        {
            DrawGizmosEvent();
        }
    }
    public AIUnit GetMainPlayer()
    {
        for (int i = 0; i < listAIs.Count; i++)
        {
            AIUnit ai = listAIs[i];
            if (ai.aiCamp==EAiCamp.MainPlayer)
            {
                return ai;
            }
        }

        return null;
    }


    /// <summary>
    /// 销毁所有AI
    /// </summary>
    public void DestroyAllAIs()
    {
        while (listAIs.Count > 0)
        {
            AIUnit ai = listAIs[0];
            DestroyAI(ai);
        }
    }

    public void DestroyAI(AIUnit ai)
    {
        if (listAIs.Contains(ai))
        {
            ai.Destroy();
            listAIs.Remove(ai);
        }
    }

    /// <summary>
    /// 为当前的AI查找相对最优先的敌人（可以有多个因素影响，距离，仇恨值等）
    /// </summary>
    /// <param name="srcAi"></param>
    /// <returns></returns>
    public AIUnit FindFirstEnemy(AIUnit srcAi)
    {
#if UNITY_EDITOR
        if (DisableAiAttack && srcAi.aiCamp == EAiCamp.Enemy)
        {
            return null;
        }
#endif
        AIUnit enemy = null;
        float distance = float.MaxValue;
        for (int i = 0; i < listAIs.Count; i++)
        {
            AIUnit ai = listAIs[i];
            if (IsAntiCamp(srcAi, ai) )
            {
                float curDist = DistanceBetween(srcAi, ai);
                if (curDist < distance)
                {
                    enemy = ai;
                    distance = curDist;
                }
            }
        }
        return enemy;
    }


    /// <summary>
    /// 判断两个AI是否反阵营的
    /// </summary>
    /// <param name="aiA"></param>
    /// <param name="aiB"></param>
    /// <returns></returns>
    public static bool IsAntiCamp(AIUnit aiA,AIUnit aiB)
    {
        if (aiA == aiB || aiA.aiCamp==aiB.aiCamp)
        {
            return false;
        }

        if (aiA.aiCamp == EAiCamp.Enemy)
        {
            if (aiB.aiCamp == EAiCamp.MainPlayer || aiB.aiCamp == EAiCamp.Friend)
            {
                return true;
            }
        }else if (aiA.aiCamp == EAiCamp.Friend || aiA.aiCamp == EAiCamp.MainPlayer)
        {
            if (aiB.aiCamp == EAiCamp.Enemy)
            {
                return true;
            }
        }

        return false;
    }

    public static AIUnit FindTarget(AIUnit srcUnit, ETargetType targetType)
    {
        if (targetType == ETargetType.MainPlayer)
        {
            return AIMgr.instance.GetMainPlayer() as AIUnit;
        }else if (targetType == ETargetType.Enemy)
        {
            return AIMgr.instance.FindFirstEnemy(srcUnit);
        }
        else
        {
            Debug.LogError("未实现的查找类型"+targetType);
            return null;
        }
    }

    public float DistanceBetween(AIUnit aiA, AIUnit aiB)
    {
        return Vector3.Distance(aiA.Position, aiB.Position);
    }
}
