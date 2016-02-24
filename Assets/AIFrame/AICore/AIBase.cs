using System.Collections.Generic;
using UnityEngine;

public class AIBase
{

    #region variables
    private GameObject mGameObj;
    private CharacterController mController;
    private Animation mAnimation;
    private CollisionFlags mCollisionFlag; //碰撞标记，可以用来区分上次移动时那个方位有碰撞
    protected float mMoveSpeed=6;  
    #endregion

    public GameObject gameObject
    {
        get { return mGameObj; }
    }

    public Transform transform
    {
        get { return mGameObj == null ? null : mGameObj.transform; }
    }

    public Vector3 Position
    {
        get { return mGameObj == null ? Vector3.zero : mGameObj.transform.position; }
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnDrawGizmos()
    {
        
    }

    public virtual void SetModel(GameObject obj)
    {
        if (mGameObj) Object.Destroy(mGameObj);
        mGameObj = obj;
        mController = mGameObj.GetComponent<CharacterController>();
        mAnimation = mGameObj.GetComponent<Animation>();
    }

    public virtual void SetModel(string resName)
    {
        GameObject obj =UnityEngine.Object.Instantiate(Resources.Load(resName)) as GameObject;
        SetModel(obj);
    }

    /// <summary>
    /// 在世界坐标进行一定的位移，有物理碰撞的话不会穿透
    /// </summary>
    /// <param name="deltaPos"></param>
    public virtual void Move(Vector3 deltaPos)
    {
       mCollisionFlag= mController.Move(deltaPos);
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

    public virtual void Destroy()
    {
        
    }
}

