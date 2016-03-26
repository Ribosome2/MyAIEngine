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
            mHitData.entityResName = EditorGUILayout.TextField("显示实体资源", mHitData.entityResName);
            mHitData.startPosition = EditorGUILayout.Vector3Field("初始位置", mHitData.startPosition);

            EditorGUILayout.Separator();
            GUILayout.Label("击中判定：");
            HitCheckBase hitCheck = mHitData.hitCheckData;
            hitCheck.shapeType = (EHitCheckShape)AIFUIUtility.DrawCustomEnum("击中框类型", hitCheck.shapeType, 100);
            hitCheck.posOffset = EditorGUILayout.Vector3Field("位置偏移", hitCheck.posOffset);
            hitCheck.radius = EditorGUILayout.FloatField("半径", hitCheck.radius);
            hitCheck.height = EditorGUILayout.FloatField("高度", hitCheck.height);
            hitCheck.angle = EditorGUILayout.FloatField("攻击角度", hitCheck.angle);


        }
    }

    void Update()
    {
        SceneView.RepaintAll();
       // HandleUtility.Repaint();
    }


    void OnDestroy()
    {
        if (mDebugDummy)
        {
            Object.DestroyImmediate(mDebugDummy);
        }
    }

    void OnFocus()
    {
        SceneView.onSceneGUIDelegate += OnSceneUI;
    }

    void OnLostFocus()
    {
        SceneView.onSceneGUIDelegate -= OnSceneUI;
    }

    void OnSceneUI(SceneView sceneView)
    {
        if (mHitData != null)
        {
            HitCheckBase hitCheck = mHitData.hitCheckData;
            Vector3 pos = debugDummy.transform.TransformPoint(mHitData.startPosition) + hitCheck.posOffset;
            Vector3 normal = debugDummy.transform.up;
            if (hitCheck.shapeType == EHitCheckShape.Fan)
            {
                Vector3 startVec = Quaternion.AngleAxis(-hitCheck.angle*0.5f, normal) * debugDummy.transform.forward;
                GizmosExtension.DrawFanShapeWithHeight(pos, normal, startVec, hitCheck.angle, hitCheck.radius, hitCheck.height);
            }else if (hitCheck.shapeType == EHitCheckShape.Capsule)
            {
                
            }
        }
        
    }

    private GameObject mDebugDummy;
    GameObject debugDummy
    {
        get
        {
            if (mDebugDummy == null)
            {
                const  string dummyName= "HitDataDebugDummy";
                mDebugDummy = GameObject.Find(dummyName);
                if (mDebugDummy == null)
                {


                    GameObject prefab = Resources.Load<GameObject>("HitDebug");
                    if (prefab == null)
                    {
                        mDebugDummy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    }
                    else
                    {
                        mDebugDummy = Object.Instantiate(prefab) as GameObject;
                    }
                    mDebugDummy.name = dummyName;
                    mDebugDummy.hideFlags = HideFlags.DontSave;
                }
            }
            return mDebugDummy;
        }
    }

    

}
