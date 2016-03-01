using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class AILinkEditWnd :EditorWindow
{
    private AIClipGroup srcGroup;
    private List<AIClip> mOptionList = new List<AIClip>();
    private Vector2 scrollPos;

    public AILink aiLink;

    public void SetData(AIClipGroup clipGroup, AILink link)
    {
        srcGroup = clipGroup;
        aiLink = link;

    }

    void OnGUI()
    {
        
    }


}
