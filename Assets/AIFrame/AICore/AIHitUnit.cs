using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
/// <summary>
/// 负责管理跟攻击相关的逻辑
/// </summary>
public class AIHitUnit
{
    public static bool ShowHitDebug=true;
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
    private float mMoveSpeed;
    private Vector3 moveDirection;

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
        if (mHitData.autoFaceTarget)
        {
            mOwner.FaceToAttackTarget();
        }
        pos = mOwner.transform.TransformPoint(mHitData.startPosition);
        eulerAngleY = mOwner.transform.eulerAngles.y;
        moveDirection = mOwner.transform.TransformDirection(mHitData.startDirection).normalized;
        mMoveSpeed = mHitData.moveSpeed;
       
        if (string.IsNullOrEmpty(mHitData.entityResName) == false)
        {
            GameObject prefab = Resources.Load(hitData.entityResName) as GameObject;
            if (prefab)
            {
                mHitEntity = Object.Instantiate(prefab) as GameObject;
                mHitEntity.transform.forward = moveDirection;
                mHitEntity.transform.position = pos;
            }
            else
            {
                Debug.LogError("加载资源"+hitData.entityResName+" 失败");
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

        if (mHitData.hitCheckData.shapeType == EHitCheckShape.LaserBeam)
        {
            HitCheckBase hitCheck = mHitData.hitCheckData;
            RaycastHit[] hits=Physics.SphereCastAll(pos, mHitData.hitCheckData.radius, moveDirection, hitCheck.height);

            if (hits != null && hits.Length > 0)
            {
                for (int hitIndex = 0; hitIndex < hits.Length; hitIndex++)
                {
                    RaycastHit hit = hits[hitIndex];
                    for (int i = 0; i < AIMgr.instance.listAIs.Count; i++)
                    {
                        AIUnit ai = AIMgr.instance.listAIs[i];
                        if (ai.Controller == hit.collider)
                        {
                            //不在攻击频率内的，跳过
                            if (IsCannotHit(ai))
                            {
                                continue;
                            }
                            else
                            {
                                ai.OnGetHit();
                                RecordHit(ai);
                            }
                        }
                    }
                }
            }
        }
        else
        {

            for (int i = 0; i < AIMgr.instance.listAIs.Count; i++)
            {
                AIUnit ai = AIMgr.instance.listAIs[i];
                //不在攻击频率内的，跳过
                if (IsCannotHit(ai))
                {
                    continue;
                }
                else
                {
                    if (ai.CheckHit(mOwner, this)) //攻击成功，要记录当前攻击时间
                    {
                        RecordHit(ai);
                    }
                }
            }
        }
    }

    private void RecordHit(AIUnit ai)
    {
        if (mHitRecord.ContainsKey(ai))
        {
            mHitRecord[ai] = Time.realtimeSinceStartup;
        }
        else
        {
            mHitRecord.Add(ai, Time.realtimeSinceStartup);
        }
    }

    private bool IsCannotHit(AIUnit ai)
    {
        return mHitRecord.ContainsKey(ai) && Time.realtimeSinceStartup - mHitRecord[ai] < mHitData.hitInterval;
    }

    void UpdateEntityPosition()
    {
        if (mHitEntity)
        {
            if (mMoveSpeed > 0)
            {
                pos += moveDirection*mMoveSpeed*Time.deltaTime;
            }
            mHitEntity.transform.position = pos;
        }
    }


    void DrawGizmos()
    {
        if (ShowHitDebug == false)
        {
            return;
        }

        if (mHitData!=null &&   mOwner!=null &&  mOwner.transform!=null)
        {
            HitCheckBase hitCheck = mHitData.hitCheckData;
            Vector3 normal = mOwner.transform.up;
            if (hitCheck.shapeType == EHitCheckShape.Fan)
            {
                
                Vector3 startVec = Quaternion.AngleAxis(-hitCheck.angle*0.5f, normal)*mOwner.transform.forward;
                GizmosExtension.DrawFanShapeWithHeight(pos, normal, startVec, hitCheck.angle,hitCheck.radius,hitCheck.height);
            }else if (hitCheck.shapeType == EHitCheckShape.Cylinder || hitCheck.shapeType == EHitCheckShape.Capsule)
            {
                GizmosExtension.DrawCylinder(pos,normal,forwardDir,hitCheck.radius,hitCheck.height);
            }else if (hitCheck.shapeType == EHitCheckShape.LaserBeam)
            {
                GizmosExtension.DrawLaserBeam(pos,  moveDirection, hitCheck.radius, hitCheck.height);
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


