using UnityEngine;
using System.Collections;

public class GizmosExtension  {


    public static void DrawFanShapeWithHeight(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius,float height)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.DrawWireArc(center,normal,from,angle,radius);
        Vector3 startPoint1 = center + from*radius;
        DrawLine(center,startPoint1);
        Vector3 endDir = Quaternion.AngleAxis(angle, normal)*from;
        Vector3 endPoint1 = center + endDir*radius;

        DrawLine(center,endPoint1);
        //上面部分
        Vector3 upperPos = center + normal.normalized*height;
        UnityEditor.Handles.DrawWireArc(upperPos, normal, from, angle, radius);
        Vector3 startPos2 = upperPos + from*radius;
        Vector3 endPoint2 = upperPos + endDir*radius;

       DrawLine(upperPos, startPos2);
       DrawLine(upperPos, endPoint2);
       DrawLine(startPoint1, startPos2);
       DrawLine(endPoint1, endPoint2);
       DrawLine(center, upperPos);
#endif

    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.DrawLine(start,end);
#endif
    }

    public static void DrawCylinder()
    {
#if UNITY_EDITOR
        //Gizmos.d
#endif
    }


}
