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

    public static void DrawCylinder(Vector3 startPoint, Vector3 normal, Vector3 dir, float radius,float height)
    {
#if UNITY_EDITOR
        normal = normal.normalized;
        float angle = 360;
        Vector3 startVec = Quaternion.AngleAxis(-angle*0.5f, normal)*dir;
        UnityEditor.Handles.DrawSolidArc(startPoint,normal,startVec,angle,radius);
        Vector3 endPoint = startPoint + normal*height;
        DrawLine(startPoint,endPoint);
        UnityEditor.Handles.DrawSolidArc(endPoint, normal, startVec, angle, radius);
        Vector3[] points=new Vector3[4];
        Vector3[] pointTops=new Vector3[4];
        float unitAngle = angle/points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 temDir= Quaternion.AngleAxis(-unitAngle*i, normal)*dir;

            points[i] = startPoint + temDir*radius;
            pointTops[i] = points[i] + normal*height;
            DrawLine(points[i],pointTops[i]);
        }

#endif
    }
    public static void DrawLaserBeam(Vector3 startPoint, Vector3 dir, float radius, float height)
    {
#if UNITY_EDITOR
        Gizmos.DrawWireSphere(startPoint,radius);
        Vector3 targetPos = startPoint + dir.normalized*height;
        Gizmos.DrawWireSphere(targetPos,radius);
        Gizmos.DrawLine(startPoint,targetPos);
#endif
    }

}
