using UnityEngine;
using System.Collections;

public class AILinkHelper {

    public static bool IsLinkConditionCheck(AILink link)
    {
        for (int i = 0; i < link.linkConditionList.Count; i++)
        {
            AILinkCondiction con = link.linkConditionList[i];
            if (con is AiInputCondiction)
            {
                AiInputCondiction input = con as AiInputCondiction;
                if (input.needDiretionInput && InputManager.HasDirectionInput==false)
                {
                    return false ;
                }

                for (int keyIndex = 0; keyIndex < input.inputStates.Count; keyIndex++)
                {
                    if (!InputManager.isKeyStateCheck(input.inputStates[keyIndex]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                Debug.LogError("Not implemented link condition");
            }
        }

        return true;
    }
}
