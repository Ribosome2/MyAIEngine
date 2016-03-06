using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI 的基础,这一层的设计只是一个基本的能运动的物品
/// </summary>
public class AIBase
{

    #region variables
    protected GameObject mGameObj;
    private CharacterController mController;
  
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

    public CharacterController Controller
    {
        get { return mController; }
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
        if (mController == null)
            mGameObj.AddComponent<CharacterController>();
        mController = mGameObj.GetComponent<CharacterController>();
      

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

    

    public virtual void Destroy()
    {
        if (gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}

