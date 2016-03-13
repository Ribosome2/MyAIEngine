using UnityEngine;
using System.Collections;
using UnityEditor;
[InitializeOnLoad] //用来进行编辑器中的初始化
public class AIFInitializer : MonoBehaviour
{

    static AIFInitializer()
    {
        Debug.Log("AIF initing ");
    }
}
