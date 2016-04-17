using System;
using UnityEngine;
using System.Collections;

public class BoundTest : MonoBehaviour
{
    public CharacterController controller;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (controller)
        {
            Bounds b = controller.bounds;
            GUILayout.Label("min"+b.min);
            GUILayout.Label("Max+"+b.max);
            GUILayout.Label("Center"+b.center);
        }
    }
}
