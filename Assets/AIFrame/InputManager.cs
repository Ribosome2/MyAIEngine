using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager>
{
    public  static Dictionary<EKeyCode, EKeyState> mKeyStates = new Dictionary<EKeyCode, EKeyState>();


    public static void Initialize()
    {
        mKeyStates.Clear();
        mKeyStates.Add(EKeyCode.MainAttack,EKeyState.KeyNormal);
        mKeyStates.Add(EKeyCode.Skill1, EKeyState.KeyNormal);
        mKeyStates.Add(EKeyCode.Skill2, EKeyState.KeyNormal);
        mKeyStates.Add(EKeyCode.Skill3, EKeyState.KeyNormal);
    }
    /// <summary>
    /// 玩家在垂直方向和水平方向的输入向量值
    /// </summary>
    public static Vector2 inputVector
    {
        get
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }


    public static void UpdateInput()
    {
       UpdateKeyState(KeyCode.F,EKeyCode.MainAttack);
       UpdateKeyState(KeyCode.J, EKeyCode.Skill1);
       UpdateKeyState(KeyCode.K, EKeyCode.Skill2);
       UpdateKeyState(KeyCode.L, EKeyCode.Skill3);
       UpdateKeyState(KeyCode.I, EKeyCode.Skill4);

    }

    static void UpdateKeyState(KeyCode keyCode, EKeyCode mapToKeyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            mKeyStates[mapToKeyCode] = EKeyState.KeyDown;
        }else if (Input.GetKeyUp(keyCode))
        {
            mKeyStates[mapToKeyCode] = EKeyState.KeyUp;
        }else if (Input.GetKey(keyCode))
        {
            mKeyStates[mapToKeyCode] = EKeyState.KeyHold;
        }
        else
        {
            mKeyStates[mapToKeyCode] = EKeyState.KeyNormal;
        }
    }


    public static bool HasDirectionInput
    {
        get { return inputVector.magnitude > 0; }
    }

    public static bool isKeyStateCheck(KeyInputState targetKeyState)
    {
        return mKeyStates[targetKeyState.keyCode] == targetKeyState.state;
    }
}
