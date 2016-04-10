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

    private static Texture2D mIconCopy;
    private static Texture2D mIconPaste;
    private static Texture2D mIconNewItem;

    public static Texture2D IconPaste
    {
        get 
        {
            if (mIconPaste == null)
            {
                mIconPaste = Resources.Load("Icons/paste") as Texture2D;
            }
            return mIconPaste;
        }
        
    }

    public static  Texture2D IconCopy
    {
        get
        {
            if (mIconCopy == null)
            {
                mIconCopy = Resources.Load<Texture2D>("Icons/copy");
            }

            return mIconCopy;
        }
    }

    public static Texture2D IconNewItem
    {
        get
        {
            if (mIconNewItem == null)
            {
                mIconNewItem = Resources.Load<Texture2D>("Icons/newItem");
            }

            return mIconNewItem;
        }
    }

}
