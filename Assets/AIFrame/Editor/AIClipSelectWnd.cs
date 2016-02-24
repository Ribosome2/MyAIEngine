using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class AIClipSelectWnd :EditorWindow
{
    private AIClipGroup srcGroup;
    private List<AIClip> mOptionList = new List<AIClip>();
    private Vector2 scrollPos;
    private AIClip mSrcClip;
    public void SetGroupData(AIClipGroup data,AIClip srcClip)
    {
        mSrcClip = srcClip;
        srcGroup = data;
        mOptionList.Clear();
        for (int i = 0; i < srcGroup.aiClipList.Count; i++)
        {
            AIClip clip = srcGroup.aiClipList[i];
            if(!IsInLinkList(srcClip,clip) && clip!=srcClip)
            { 
              //we only need those not in the list
                mOptionList.Add(clip);
            }

        }
    }

    bool IsInLinkList(AIClip targetClip, AIClip clip)
    {
        for (int i = 0; i < targetClip.linkAIClipList.Count; i++)
        {
            if (targetClip.linkAIClipList[i].linkToClip == clip)
            {
                return true;
            }
        }
        return false;
    }

    void OnGUI()
    {
        if (srcGroup != null)
        {
            
            for (int i = 0; i <mOptionList.Count; i++)
            {
                AIClip clip = mOptionList[i];
                if (GUILayout.Button(clip.name))
                {
                    AILinkClip linkClip = new AILinkClip();
                    linkClip.linkToClip = clip;
                    mSrcClip.linkAIClipList.Add(linkClip);
                    EditorWindow.GetWindow<AIDataEditor>().Repaint();
                    Close();
                }
            }
        }
    }


}
