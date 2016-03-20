using UnityEngine;
using System.Collections;

public class GizmosExtension  {


    public static void DrawFanShapeWithHeight(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius,float height)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.DrawWireArc(center,normal,from,angle,radius);
        Vector3 startPoint1 = center + from*radius;
        Gizmos.DrawLine(center,startPoint1);
        Vector3 endDir = Quaternion.AngleAxis(angle, normal)*from;
        Vector3 endPoint1 = center + endDir*radius;

        Gizmos.DrawLine(center,endPoint1);
        //上面部分
        Vector3 upperPos = center + normal.normalized*height;
        UnityEditor.Handles.DrawWireArc(upperPos, normal, from, angle, radius);
        Vector3 startPos2 = upperPos + from*radius;
        Vector3 endPoint2 = upperPos + endDir*radius;

        Gizmos.DrawLine(upperPos,startPos2 );
        Gizmos.DrawLine(upperPos,endPoint2 );
        Gizmos.DrawLine(startPoint1,startPos2);
        Gizmos.DrawLine(endPoint1,endPoint2);
        Gizmos.DrawLine(center,upperPos);
#endif

    }

}
