using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager>
{
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
   

   

}
