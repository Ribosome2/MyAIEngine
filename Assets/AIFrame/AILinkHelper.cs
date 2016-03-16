using UnityEngine;
using System.Collections;

public class AILinkHelper {

    public static bool IsLinkConditionCheck(AILink link,AIUnit linkSrc)
    {
        for (int i = 0; i < link.linkConditionList.Count; i++)
        {
            AILinkCondiction con = link.linkConditionList[i];
            if (con is AiInputCondiction)
            {
                AiInputCondiction input = con as AiInputCondiction;
                if (!CheckInputCondition(input)) return false;
            }
            else if (con is AiTargetStateCondiction)
            {
                AiTargetStateCondiction targetState = con as AiTargetStateCondiction;
                if (!CheckTargetState(targetState, linkSrc))
                {
                    return false;
                }
            }
            else
            {
                Debug.LogError("Not implemented link condition");
            }
        }

        return true;
    }


    /// <summary>
    /// 检测输入条件是否满足
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool CheckInputCondition(AiInputCondiction input)
    {
        if (input.needDiretionInput && InputManager.HasDirectionInput == false)
        {
            return false;
        }

        for (int keyIndex = 0; keyIndex < input.inputStates.Count; keyIndex++)
        {
            if (!InputManager.isKeyStateCheck(input.inputStates[keyIndex]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckTargetState(AiTargetStateCondiction targetCon, AIUnit owner)
    {
        AIUnit target = null;

        #region 根据目标类型找到目标
        if (targetCon.targetType == ETargetType.Enemy)
        {
            target = AIMgr.instance.FindFirstEnemy(owner);
        }
        else if (targetCon.targetType == ETargetType.MainPlayer)
        {
            target = AIMgr.instance.GetMainPlayer() as AIUnit;
        }
        else
        {
            Debug.LogError("未实现的角色类型" + targetCon.targetType);
        } 
        #endregion

        if (targetCon.targetState == ETargetState.Lost && target == null)
        {
            return true;
        }

        if (targetCon.targetState == ETargetState.InSight && target != null)
        {
            return true;
        }

        if (target != null)
        {
            float dist = AIMgr.instance.DistanceBetween(target, owner);
            if (targetCon.targetState == ETargetState.InAttackRange && dist <= targetCon.targetDistance)
            {
                return true;
            }
            else if (targetCon.targetState == ETargetState.OutOfRange && dist > targetCon.targetDistance)
            {
                return true;
            }
        }



        return false;
    }

}
