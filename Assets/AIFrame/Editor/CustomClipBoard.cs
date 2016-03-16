using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CustomClipBoard
{
    private static object mCopyData=null;
    public static void CopyData(object dataToCopy )
    {
        mCopyData = dataToCopy;
        Debug.Log("成功复制"+mCopyData.ToString());
    }

    public static void GetCopyObject(out object targetObject)
    {
        targetObject = mCopyData;
        mCopyData = null;
    }

	
}
