using UnityEngine;
using System.Collections;
[System.Serializable]
public class AiInputCondiction:AILinkCondiction
{
    /// <summary>
    /// //是否需要方向输入，前后左后的输入向量不为0
    /// </summary>
    public bool needDiretionInput;

}
