using System.Collections.Generic;
using UnityEngine;
using System.Collections;
/// <summary>
/// 负责管理跟攻击相关的逻辑
/// </summary>
public class AIHitUnit
{

    /// <summary>
    /// 伤害外形物体
    /// </summary>
    protected GameObject mHitEntity;
    /// <summary>
    /// 攻击定义数据
    /// </summary>
    protected AiClipHitData mHitData;
    Dictionary<AIUnit, float> mHitRecord = new Dictionary<AIUnit, float>();
    protected AIUnit mOwner;
    protected Vector3 pos;
    private float mLiveTime;
    private int mCurHitCount;//击中次数
    public bool ShouldDie
    {
        get { return mLiveTime >= mHitData.lastTime; }
    }

    public void Init(AiClipHitData hitData,AIUnit owner)
    {
        mHitData = hitData;
        mOwner = owner;
        
        pos = mOwner.transform.TransformPoint(mHitData.startPosition);
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
                if (ai.CheckHit(mOwner, mHitData))//攻击成功，要记录当前攻击时间
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

    public void Destroy()
    {
        if (mHitEntity)
        {
            Object.Destroy(mHitEntity);
        }
    }
 
}


