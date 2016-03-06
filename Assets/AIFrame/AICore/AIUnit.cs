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

    public override void SetModel(GameObject obj)
    {
        base.SetModel(obj);
        mAnimation = mGameObj.GetComponent<Animation>();
        UpdateShape();
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
        mCurAIClip = aiClip;
    }

    public virtual void SwitchAIClipByName(string  clipName)
    {
        AIClip clip = mAiClipGroup.aiClipList.Find(delegate(AIClip targetClip)
        {
            return targetClip.animationName == clipName;
        });
        SwitchAIClip(clip);

    }


    public override void Move(Vector3 deltaPos)
    {
        base.Move(deltaPos);
        ChooseMovementAnimation(deltaPos);
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
                PlayAnimation(AiGroupData.commonAnimation.idle, 0.1f);
            }
            else
            {
                PlayAnimation(AiGroupData.commonAnimation.run, 0.1f);
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
}
