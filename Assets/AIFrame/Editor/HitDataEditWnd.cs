using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class HitDataEditWnd : EditorWindow {

    public static void Open(AiClipHitData hitData)
    {
        HitDataEditWnd wnd = EditorWindow.GetWindow<HitDataEditWnd>();
        wnd.SetData(hitData);
    }

  
    private AiClipHitData mHitData;

    public void SetData(AiClipHitData hitData)
    {
        mHitData = hitData;
    }

    public void OnGUI()
    {
        if (mHitData != null)
        {
            mHitData.name = AIFUIUtility.DrawTextField(mHitData.name, "名称", 100);
            mHitData.startTime = EditorGUILayout.FloatField("开始触发时间",mHitData.startTime );
            mHitData.lastTime = EditorGUILayout.FloatField( "持续时间",mHitData.lastTime);
            mHitData.hitInterval = EditorGUILayout.FloatField("攻击间隔", mHitData.hitInterval);

            EditorGUILayout.Separator();
            GUILayout.Label("击中判定：");
            HitCheckBase hitCheck = mHitData.hitCheckData;
            hitCheck.shapeType = (EHitCheckShape)AIFUIUtility.DrawCustomEnum("击中框类型", hitCheck.shapeType, 100);
            hitCheck.posOffset = EditorGUILayout.Vector3Field("位置偏移", hitCheck.posOffset);
            hitCheck.radius = EditorGUILayout.FloatField("半径", hitCheck.radius);
            hitCheck.height = EditorGUILayout.FloatField("高度", hitCheck.height);
            

        }
    }

}
