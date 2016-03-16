using UnityEngine;
using System.Collections;

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
        UpdateShape();
    }

    public virtual void SwitchAIClip(AIClip aiClip)
    {

        if (aiClip == null)
        {
            Debug.LogError("不能设置空数据");
            return;
        }
        if (mCurAIClip.animationName != aiClip.animationName)
        {
           
            Debug.Log("Switcing to " + aiClip.animationName);
            PlayAnimation(aiClip.animationName, 0);
        }
        mCurAIClip = aiClip;
      
    }

    public virtual void SwitchAIClipByClipKey(string  clipKey)
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
        SwitchAIClip(clip);
        
    }


    public override void Move(Vector3 deltaPos)
    {
        base.Move(deltaPos);
        
        
        
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
                SwitchAIClipByClipKey(link.linkToClip);
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
        if (UseMecanimAnimation)
        {
            animator.Play(clipName);
            //animator.CrossFade(clipName, fadeTime);
        }
        else
        {
            mAnimation.CrossFade(clipName, fadeTime);
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
        deltaPos = (deltaPos + Physics.gravity) * Time.deltaTime;
        return deltaPos;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Position, Position + relativeForward * 5);
        Gizmos.DrawLine(Position, Position + relativeRight * 5);
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

}
