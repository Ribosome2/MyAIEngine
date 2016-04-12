
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

/// <summary>
/// 继承AIBase 的基本物体属性， 同时这一层开始有多些外观表现，比如有它的父类没有的动画
/// </summary>
public class AIUnit : AIBase
{
    public static bool UseMecanimAnimation = true; //设置是用Mecanim 动画系统还是旧的动画系统
    private Animation mAnimation;
    private AIClip mCurAIClip; //当前所在的AI片断
    private AIDataUnit mDataUnit;
    private AIClipGroup mAiClipGroup;

    private Animator mAnimator;
    private float mCurClipTime;

    public float CurClipTime
    {
        get { return mCurClipTime; }
            
      
    }

    public Animator animator
    {
        get
        {
            if (mAnimator == null)
            {
                mAnimator = gameObject.GetComponent<Animator>();
            }
            return mAnimator;
        }
    }

    public AIClipGroup AiGroupData
    {
        get { return mAiClipGroup; }
    }

    public string Name
    {
        get
        {
            if (mAiClipGroup != null)
            {
                return mAiClipGroup.GroupName;
            }
            return "no name";
        }
    }
    public AIClip CurAiClip
    {
        get { return mCurAIClip; }
    }

    public EAiCamp aiCamp { get; set; }
    private AIHitManager mHitManager;
    private AIEventController mEventController;
    ExtrenalVelocity mExtrenalVelocity=new ExtrenalVelocity();
    List<AIEventListener> mEventListeners=new List<AIEventListener>();
    private string curAnimationName="";
    public AIUnit()
    {
        mHitManager=new AIHitManager(this);
        mEventController=new AIEventController(this);
    }

    public void AddEventListener(AIEventListener listener)
    {
        if (listener != null && mEventListeners.Contains(listener) == false)
        {
            mEventListeners.Add(listener);
        }
    }

    public void RemoveEventListener(AIEventListener listener)
    {
        if (mEventListeners.Contains(listener))
        {
            mEventListeners.Remove(listener);
        }
    }

    public void NotifyEvent(EAiEventType type)
    {
        for (int i = 0; i < mEventListeners.Count; i++)
        {
            AIEventListener listener = mEventListeners[i];
            listener.OnEvent(this,type);
        }
    }

    public void NotifyAiUpdate(float dt)
    {
        for (int i = 0; i < mEventListeners.Count; i++)
        {
            AIEventListener listener = mEventListeners[i];
            listener.OnUpdateAI(dt);
        }
    }

    public override void SetModel(GameObject obj)
    {
        base.SetModel(obj);
        mAnimation = mGameObj.GetComponent<Animation>();
        UpdateShape();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        CheckClipFinish(deltaTime);

        CheckLinkClips();
        Vector3 deltaPos = CalculateMoveDelta(deltaTime);
        Move(deltaPos);
        if (mCurAIClip.CheckDirectionInput)
        {
            ChooseMovementAnimation(deltaPos);
        }
        NotifyAiUpdate(deltaTime);
    }

    public void UpdateShape()
    {
        if (Controller && mAiClipGroup!=null)
        {
            AIShape shape = mAiClipGroup.shape;
            Controller.center = shape.colliderOffset;
            Controller.height = shape.colliderHeight;
            Controller.radius = shape.colliderRadius;
        }
    }


    public virtual void SetAIDataUnit(AIDataUnit aiData)
    {
        mDataUnit = aiData;
        
        SetAIClipGroup(mDataUnit.aiGroups[0]);
    }

    public void SwitchAI(bool useAi)
    {
        if (useAi && mDataUnit.aiGroups.Count > 1)
        {
            SetAIClipGroup(mDataUnit.aiGroups[1]);
        }
        else
        {
            SetAIClipGroup(mDataUnit.aiGroups[0]);
        }
    }

    /// <summary>
    /// 设置AI组数据
    /// </summary>
    public virtual void SetAIClipGroup(AIClipGroup groupData)
    {
        mAiClipGroup = groupData;
        mCurAIClip = mAiClipGroup.aiClipList[0];
        SwitchAIClip(mCurAIClip,0);
        UpdateShape();
    }

    public virtual void SwitchAIClip(AIClip aiClip,float fadeTime)
    {

        if (aiClip == null)
        {
            Debug.LogError("不能设置空数据");
            return;
        }
        if (curAnimationName != aiClip.animationName)
        {
           
            Debug.Log("Switcing to " + aiClip.animationName);
            PlayAnimation(aiClip.animationName, fadeTime);
        }
        animator.applyRootMotion = aiClip.applyRootMotion;
        mCurAIClip = aiClip;
        NotifyEvent(EAiEventType.SwitchAiClip);
      
    }

    public virtual void SwitchAIClipByClipKey(string  clipKey,float fadeTime=0.1f)
    {
        if (CurAiClip != null && CurAiClip.clipKey == clipKey) //当前已经是在目标片断， 不要重复切换
        {
            return;
        }
        mCurClipTime = 0;

        AIClip clip = mAiClipGroup.aiClipList.Find(delegate(AIClip targetClip)
        {
            return targetClip.clipKey == clipKey;
        });
        SwitchAIClip(clip,fadeTime);
        
    }


    public void CheckClipFinish(float deltaTime)
    {
        mCurClipTime += deltaTime;
        //动画片断时间到，选择连接片断
        if (mCurClipTime >= mCurAIClip.animationTime)
        {
            SwitchAIClipByClipKey(mCurAIClip.defaultLinkClip);
        }
    }


    /// <summary>
    /// 检测是否可以连接到其他的片断
    /// </summary>
    public void CheckLinkClips()
    {
        for (int i = 0; i < mCurAIClip.linkAIClipList.Count; i++)
        {
            AILink link = mCurAIClip.linkAIClipList[i];
            if (AILinkHelper.IsLinkConditionCheck(link,this))
            {
                SwitchAIClipByClipKey(link.linkToClip,link.crossFadeTime);
                break;
            }
        }
    }



    /// <summary>
    /// 根据移动向量来选择移动的动画
    /// </summary>
    /// <param name="deltaMove"></param>
    public virtual void ChooseMovementAnimation(Vector3 deltaMove)
    {
        if (AiGroupData != null)
        {
            if (deltaMove.x == 0 &&  deltaMove.z == 0)
            {
                
                SwitchAIClipByClipKey(AiGroupData.commonAnimation.idle);
            }
            else
            {
                SwitchAIClipByClipKey(AiGroupData.commonAnimation.run);
                FaceToDirection(deltaMove);
            }
        }
      
    }

    /// <summary>
    /// 播放制定的动画
    /// </summary>
    /// <param name="clipName">动画片断名称</param>
    /// <param name="fadeTime">过度时间</param>
    public virtual void PlayAnimation(string clipName, float fadeTime)
    {
        curAnimationName = clipName;
        if (UseMecanimAnimation)
        {
            //用CrossFade方法好像有时切换不到目标片断，好烦，
            animator.Play(clipName);
           // animator.CrossFade(clipName, fadeTime);
        }
        else
        {
            mAnimation.CrossFade(clipName, fadeTime);
        }
    }

    /// <summary>
    /// 如果当前有攻击目标，转向攻击目标
    /// </summary>
    public void FaceToAttackTarget()
    {
        AIUnit target = AIMgr.instance.FindFirstEnemy(this);
        if (target != null)
        {
            Vector3 dir = target.Position - Position;
            Vector3 dirWitoutY = new Vector3(dir.x, 0, dir.z).normalized;

            transform.rotation = Quaternion.LookRotation(dirWitoutY);
        }
       
    }

    public void FaceToDirection(Vector3 dir)
    {
        Vector3 dirWitoutY = new Vector3(dir.x, 0, dir.z).normalized;

        transform.rotation =Quaternion.LookRotation(dirWitoutY);
    }

    private Vector3 CalculateMoveDelta(float deltaTime)
    {
        Vector3 deltaPos=Vector3.zero;
        mMoveSpeed = AiGroupData.moveSpeed;
        if (CurAiClip.CheckDirectionInput) //用输入控制
        {
            deltaPos=new Vector3(InputManager.inputVector.x, 0,
                InputManager.inputVector.y);
            deltaPos = (relativeForward*deltaPos.z + relativeRight*deltaPos.x)*mMoveSpeed;
            deltaPos.y = 0;
            
        }else if (CurAiClip.runToTarget) //跑向目标
        {
            //暂时直接往目标方向，
            //todo 算出实际寻路方向
            AIUnit target = AIMgr.instance.FindFirstEnemy(this);
            if (target != null)
            {
                deltaPos = (target.Position - Position).normalized*mMoveSpeed;
                deltaPos.y = 0;
                FaceToDirection(deltaPos);
            }
        }
        deltaPos = (deltaPos + Physics.gravity) * deltaTime;
        deltaPos += mExtrenalVelocity.GetMovementByDeltaTime(deltaTime);
        return deltaPos;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(Position, Position + relativeForward * 5);
        //Gizmos.DrawLine(Position, Position + relativeRight * 5);
    }

    /// <summary>
    /// 相对于主摄像机的向前方向
    /// </summary>
    public Vector3 relativeForward
    {
        get
        {
            Vector3 forwad = Camera.main.transform.forward;
            forwad.y = 0;
            return forwad.normalized;
        }
    }

    public Vector3 relativeRight
    {
        get
        {
            Vector3 right = Camera.main.transform.right;
            right.y = 0;
            return right.normalized;
        }
    }


  




    public override void Destroy()
    {
        base.Destroy();
        NotifyEvent(EAiEventType.Dead);
    }

    #region 供外部调用的函数
    public bool CheckHit(AIUnit attacker, AIHitUnit hitUnit)
    {
        if (AIMgr.IsAntiCamp(attacker, this) == false)
        {
            return false;
        }
        bool isHit = false;
        AiClipHitData hitData = hitUnit.mHitData;
        HitCheckBase hitCheck = hitData.hitCheckData;
        switch (hitData.hitCheckData.shapeType)
        {
            case EHitCheckShape.Capsule:
            case EHitCheckShape.Cylinder:
                {
                    if (MathTool.CheckCylinderHit(hitUnit.pos, hitData.hitCheckData.height, hitData.hitCheckData.radius, Controller))
                    {
                        isHit = true;
                    }
                    break;
                }
            case EHitCheckShape.Fan:
                {
                    if (MathTool.CheckFanHit(hitUnit.pos, hitCheck.height, hitCheck.radius, hitCheck.angle, hitUnit.forwardDir, Controller))
                    {
                        isHit = true;
                    }
                    break;
                }
        }

        if (isHit)
        {
            SwitchAIClipByClipKey(mAiClipGroup.commonAnimation.hit);
        }

        return isHit;

    }

    /// <summary>
    /// 设置相对于自己本身的速度
    /// </summary>
    /// <param name="relativeVel"></param>
    public void SetExtrenalVelocity(Vector3 relativeVel)
    {
        mExtrenalVelocity.SetVelocity(transform.TransformVector(relativeVel));
    }
    #endregion

}

/// <summary>
/// 外部速度，用来增加受力感觉
/// </summary>
public class ExtrenalVelocity
{
    private Vector3 unitDir;
    private float magnitude;
    Vector3 moveDelta=Vector3.zero;
    public void SetVelocity(Vector3 vel)
    {
        unitDir = vel.normalized;
        magnitude = vel.magnitude;
    }

    public Vector3 GetMovementByDeltaTime(float dt)
    {
         moveDelta=Vector3.zero;
        if (magnitude >= 0)
        {
            moveDelta=unitDir*dt*magnitude;
            //每次都会衰减速度， 所以这个函数也是应该每次运动都调用的
            magnitude = Mathf.Max(0, magnitude - dt);
        }
        return moveDelta;
    }
}
