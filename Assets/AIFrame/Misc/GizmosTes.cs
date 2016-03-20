using UnityEngine;
using System.Collections;

public class GizmosTes : MonoBehaviour
{
    public Vector3 offset;
    public float radius = 0.5f;
    public float height = 0.2f;
    public int angle = 37;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Vector3 pos = transform.position + offset;
        GizmosExtension.DrawFanShapeWithHeight(pos,transform.up,transform.right,angle,radius,height);
    }
}
