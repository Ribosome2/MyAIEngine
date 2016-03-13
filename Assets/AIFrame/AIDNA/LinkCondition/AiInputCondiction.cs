using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EKeyState
{
    KeyDown,
    KeyHold,
    KeyUp,
    KeyNormal,
}

public enum EKeyCode
{
    MainAttack=1,
    Skill1=2,
    Skill2=3,
    Skill3=4,
    Skill4=5,
}


public class KeyInputState
{
    public EKeyCode keyCode=EKeyCode.MainAttack;
    public EKeyState state=EKeyState.KeyDown;
}

[System.Serializable]
[AIFCondition("输入条件",typeof(AiInputCondiction))]
public class AiInputCondiction:AILinkCondiction
{
    /// <summary>
    /// //是否需要方向输入，前后左后的输入向量不为0
    /// </summary>
    public bool needDiretionInput;

    /// <summary>
    /// 要求的按键输入
    /// </summary>
    public List<KeyInputState> inputStates=new List<KeyInputState>();

#if UNITY_EDITOR
    public override void OnEditorUI()
    {
        base.OnEditorUI();
        needDiretionInput = GUILayout.Toggle(needDiretionInput, "要求方向输入", GUILayout.Width(150));
        if (GUILayout.Button("增加按键输入条件",GUILayout.Width(200)))
        {
            inputStates.Add(new KeyInputState());
        }
        for (int i = 0; i < inputStates.Count; i++)
        {
            KeyInputState keyState = inputStates[i];
            GUILayout.BeginHorizontal();
            keyState.keyCode = (EKeyCode)EditorGUILayout.EnumPopup(keyState.keyCode, GUILayout.Width(80));
            keyState.state = (EKeyState) EditorGUILayout.EnumPopup(keyState.state, GUILayout.Width(80));
            if(GUILayout.Button("X",GUILayout.Width(30)))
            {
                if (EditorUtility.DisplayDialog("提示", "删除输入条件？", "确定"))
                {
                    inputStates.Remove(keyState);
                    break;
                }
            }
            GUILayout.EndHorizontal();

        }
    }
#endif
}
