using UnityEngine;
using System.Collections;

/// <summary>
/// 继承AIBase 的基本物体属性， 同时这一层开始有多些外观表现，比如有它的父类没有的动画
/// </summary>
public class AIUnit : AIBase
{
    private Animation mAnimation;
    private AIClip mCurAIClip; //当前所在的AI片断

    private AIClipGroup mAiClipGroup;

    public AIClipGroup AiGroupData
    {
        get { return mAiClipGroup; }
    }

    public override void SetModel(GameObject obj)
    {
        base.SetModel(obj);
        mAnimation = mGameObj.GetComponent<Animation>();
    }
    /// <summary>
    /// 设置AI组数据
    /// </summary>
    public virtual void SetAIClipData(AIClipGroup groupData)
    {
        mAiClipGroup = groupData;
        mCurAIClip = mAiClipGroup.aiClipList[0];
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
        if (deltaMove.x == 0 || deltaMove.z == 0)
        {
            PlayAnimation("idle", 0.3f);
        }
        else
        {
            PlayAnimation("run", 0.3f);
        }
    }

    /// <summary>
    /// 播放制定的动画
    /// </summary>
    /// <param name="clipName">动画片断名称</param>
    /// <param name="fadeTime">过度时间</param>
    public virtual void PlayAnimation(string clipName, float fadeTime)
    {
        mAnimation.CrossFade(clipName, fadeTime);
    }


}
