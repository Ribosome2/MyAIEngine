using System.Collections.Generic;
using UnityEngine;
using System.Collections;
/// <summary>
/// 负责管理跟攻击相关的逻辑
/// </summary>
public class AIHitUnit
{
    public AIHitUnit()
    {
        AIMgr.instance.DrawGizmosEvent += DrawGizmos;
    }
    /// <summary>
    /// 伤害外形物体
    /// </summary>
    protected GameObject mHitEntity;
    /// <summary>
    /// 攻击定义数据
    /// </summary>
    public  AiClipHitData mHitData;
    Dictionary<AIUnit, float> mHitRecord = new Dictionary<AIUnit, float>();
    protected AIUnit mOwner;
    public  Vector3 pos;
    private float mLiveTime;
    private int mCurHitCount;//击中次数
    private float eulerAngleY;

    public Vector3 forwardDir
    {
        get { return mOwner.transform.forward; }
    }

    public bool ShouldDie
    {
        get { return mLiveTime >= mHitData.lastTime; }
    }

    public void Init(AiClipHitData hitData,AIUnit owner)
    {
        mHitData = hitData;
        mOwner = owner;
        
        pos = mOwner.transform.TransformPoint(mHitData.startPosition);
        eulerAngleY = mOwner.transform.eulerAngles.y;
        if (string.IsNullOrEmpty(mHitData.entityResName) == false)
        {
            GameObject prefab = Resources.Load(hitData.entityResName) as GameObject;
            if (prefab)
            {
                mHitEntity = Object.Instantiate(prefab) as GameObject;
                mHitEntity.transform.position = pos;
            }
        }
    }

    public virtual void OnUpdate(float deltaTime)
    {
        mLiveTime += deltaTime;
        UpdateEntityPosition();
        CheckHit();
    }

    void CheckHit()
    {
        for (int i = 0; i < AIMgr.instance.listAIs.Count; i++)
        {
            AIUnit ai = AIMgr.instance.listAIs[i];
            //不在攻击频率内的，跳过
            if (mHitRecord.ContainsKey(ai) && Time.realtimeSinceStartup - mHitRecord[ai] < mHitData.hitInterval)
            {
                continue;
            }
            else
            {
                if (ai.CheckHit(mOwner,this))//攻击成功，要记录当前攻击时间
                {
                    if (mHitRecord.ContainsKey(ai))
                    {
                        mHitRecord[ai] = Time.realtimeSinceStartup;
                    }
                    else
                    {
                        mHitRecord.Add(ai,Time.realtimeSinceStartup);
                    }
                }
            }
        }
    }

    void UpdateEntityPosition()
    {
        if (mHitEntity)
        {
            mHitEntity.transform.position = pos;
        }
    }


    void DrawGizmos()
    {
        if (mHitData!=null)
        {
            HitCheckBase hitCheck = mHitData.hitCheckData;
            if (hitCheck.shapeType == EHitCheckShape.Fan)
            {
                Vector3 normal = mOwner.transform.up;
                Vector3 startVec = Quaternion.AngleAxis(-hitCheck.angle*0.5f, normal)*mOwner.transform.forward;
                GizmosExtension.DrawFanShapeWithHeight(pos, normal, startVec, hitCheck.angle,hitCheck.radius,hitCheck.height);
            }
        }
    }

    public void Destroy()
    {
        if (mHitEntity)
        {
            Object.Destroy(mHitEntity);
        }

        AIMgr.instance.DrawGizmosEvent -= DrawGizmos;
    }
 
}


