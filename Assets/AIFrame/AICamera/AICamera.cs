using UnityEngine;
using System.Collections;

public class AICamera :MonoBehaviour
{
    public AIBase target
    {
        get { return AIMgr.instance.GetMainPlayer(); }
    }
    private Camera mMainCamera;
    public float yOffset =2;
    public Vector2 mouseRotateSpeed=new Vector2(50,20);
    public float distance=8;
    private float angleX;
    private float angleY;
    private float yAngleMax=80;
    private float yAngleMin = 20;
    public void Update()
    {
        AIBase followTaget = target;
        if (followTaget != null && followTaget.transform != null)
        {
            Transform targetTran = followTaget.transform;
            Vector3 eulerAngles = targetTran.eulerAngles;
             
            if (Input.GetMouseButton(1))
            {
                angleX += Input.GetAxis("Mouse X") * mouseRotateSpeed.x*Time.deltaTime;
                angleY -= Input.GetAxis("Mouse Y") * mouseRotateSpeed.y*Time.deltaTime;
                angleY = ClampAngle(angleY, yAngleMin, yAngleMax);
            }
            Quaternion rotation = Quaternion.Euler(angleY, angleX, 0);


            float scrollValue = Input.GetAxis("Mouse ScrollWheel");
            
            distance -= scrollValue;
            
            
            //以跟随目标为中心，向指定欧拉角方向偏移一定距离， 得出当前摄像机应该要在的坐标
            Vector3 targetPos = rotation*new Vector3(0, yOffset, -distance) + targetTran.position;
            Camera.main.transform.position = targetPos;
            Camera.main.transform.rotation = rotation;
            
        }
    }

   

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360f;
        if (angle > 360)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
