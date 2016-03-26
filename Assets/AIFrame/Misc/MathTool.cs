using UnityEngine;
using System.Collections;

public class MathTool {
    public static bool CheckCylinderHit(Vector3 hitPos, float height, float radius, CharacterController controller)
    {
        Bounds targetBound = controller.bounds;
        if (targetBound.min.y < hitPos.y || targetBound.max.y > hitPos.y + height)
        {  //
            return false;
        }

        float deltaX = targetBound.center.x - hitPos.x;
        float deltaZ = targetBound.center.z - hitPos.z;

        Vector3 pos = targetBound.center;
        pos=new Vector3(pos.x,hitPos.y,pos.x);
        if (Vector3.Distance(hitPos, pos) > radius + controller.radius)
        {
            return false;
        }

        return true;
    }

    public static bool CheckFanHit(Vector3 hitPos, float height, float radius,float Angle,Vector3 direction, CharacterController controller)
    {
        Bounds targetBound = controller.bounds;
        if (targetBound.max.y < hitPos.y || targetBound.min.y > hitPos.y + height)
        {  //
            return false;
        }

      

        Vector3 pos = controller.transform.position;
        pos = new Vector3(pos.x, hitPos.y, pos.z);
        if (Vector3.Distance(hitPos, pos) > radius + controller.radius)
        {
            //不在半径范围
            return false;
        }

        Vector3 relativeVector = pos - hitPos;
        if (Mathf.Abs(Vector3.Angle(direction, relativeVector)) > Angle*0.5f)
        {
            //不在夹角范围
            return false;
        }

        return true;
    }
}
