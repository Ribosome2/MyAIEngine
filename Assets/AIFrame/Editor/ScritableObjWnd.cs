using UnityEditor;
using UnityEngine;
using System.Collections;

public class ScritableObjWnd : EditorWindow {

    [MenuItem("ScritableObj/Create")]
    public static void Create()
    {
        string className = "roleInfo";
        ScriptableObject scriptableObject = ScriptableObject.CreateInstance(className);
        AssetDatabase.CreateAsset(scriptableObject, outPutDataPath + className+".asset");
        AssetDatabase.Refresh();
    }

    private static string outPutDataPath
    {
        get { return Application.dataPath + "/AIFrame/TableData/"; }
    }
}
