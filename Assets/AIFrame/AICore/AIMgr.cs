using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMgr :Singleton<AIMgr>
{
    public List<AIBase> listAIs = new List<AIBase>();

    public void CreateAI(string resName)
    {
        Object obj = Resources.Load(resName);
        if (obj != null)
        {
            GameObject aiModel = Object.Instantiate(obj) as GameObject;
            AIForMainPlayer ai = new AIForMainPlayer();
            ai.SetModel(aiModel);
            listAIs.Add(ai);
        }
        else
        {
            Debug.LogError(string.Format("资源{0}不存在",resName));
        }
    }

    public void OnUpdate()
    {
        for (int i = 0; i < listAIs.Count; i++)
        {
            listAIs[i].OnUpdate();
        }
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < listAIs.Count; i++)
        {
            listAIs[i].OnDrawGizmos();
        }
    }
    public AIBase GetMainPlayer()
    {
        for (int i = 0; i < listAIs.Count; i++)
        {
            AIBase ai = listAIs[i];
            if (ai is AIForMainPlayer)
            {
                return ai;
            }
        }

        return null;
    }

    


    
}
