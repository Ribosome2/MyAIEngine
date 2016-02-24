using UnityEngine;
using System.Collections;

/// <summary>
/// 由玩家自己控制的AI
/// </summary>
public class AIForMainPlayer :AIBase {

    public override void OnUpdate()
    {
        base.OnUpdate();

        Move(CalculateMoveDelta());
    }

    private Vector3 CalculateMoveDelta()
    {
        Vector3 deltaPos = new Vector3(InputManager.inputVector.x, 0,
            InputManager.inputVector.y);
        deltaPos = (relativeForward * deltaPos.z + relativeRight * deltaPos.x) * mMoveSpeed;
        deltaPos.y = 0;
        deltaPos = (deltaPos + Physics.gravity)*Time.deltaTime;

        return deltaPos;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Position, Position + relativeForward*5);
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
