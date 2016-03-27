using UnityEngine;
using System.Collections;

public class TestAIScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    AIMgr.instance.OnUpdate();
	}

    public void OnDrawGizmos()
    {
        AIMgr.instance.OnDrawGizmos();
    }

    void OnGUI()
    {
       AIHitUnit.ShowHitDebug= GUILayout.Toggle(AIHitUnit.ShowHitDebug, "显示攻击调试");
       GUILayout.Label("普攻: F");
       GUILayout.Label("技能1：J");
        GUILayout.Label("技能2：K");
        GUILayout.Label("技能3：L");
        GUILayout.Label("技能4：I");
    }

    
}
