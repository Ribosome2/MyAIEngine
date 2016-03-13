using UnityEngine;
using System.Collections;

public class AIFGUISKin : MonoBehaviour {

    private static  GUISkin mCustomSkin;
    public static  GUISkin customSkin
    {
        get
        {
            if (mCustomSkin == null)
            {
                mCustomSkin = Resources.Load("CustomSkin") as GUISkin;
            }
            return mCustomSkin;
        }
    }

    public static GUIStyle GetStyle(string styleName)
    {
        return customSkin.FindStyle(styleName);
    }
}
