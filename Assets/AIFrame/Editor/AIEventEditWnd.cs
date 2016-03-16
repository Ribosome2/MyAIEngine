using System.Runtime.Serialization.Formatters;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class AIEventEditWnd : EditorWindow
{
    public  delegate void OnCreateEvent(AIClipEvent aiEvent);

    public OnCreateEvent onCreate;

    public AIClipEvent mCurEvet;
    private Vector2 scrollPos;

    public static void OpenCreateEditor(OnCreateEvent callBack)
    {
        AIEventEditWnd wnd = EditorWindow.GetWindow<AIEventEditWnd>();
        wnd.OpenAsCreateMode();
        wnd.onCreate = callBack;
    }

    public void OpenAsCreateMode()
    {
        mCurEvet = null;
    }

    public static void OpenAsEditMode(AIClipEvent clipEvent)
    {
        AIEventEditWnd wnd = EditorWindow.GetWindow<AIEventEditWnd>();
        wnd.OpenAsCreateMode();
        wnd.mCurEvet = clipEvent;

    }

    void OnGUI()
    {
        if (mCurEvet == null) //没创建实例事件，是在创建过程
        {
            DrawCreateUI();
        }
        else
        {
            DrawEditUI();
        }
    }

    void DrawEditUI()
    {
        GUILayout.Label("事件类型 : "+mCurEvet.ToString());
        mCurEvet.eventName = EditorGUILayout.TextField("事件名称" , mCurEvet.eventName);
        mCurEvet.triggerTime = EditorGUILayout.FloatField("触发时间", mCurEvet.triggerTime);
        if (mCurEvet is ShowEffectEvent)
        {
            ShowEffectEvent effectEvent = (ShowEffectEvent) mCurEvet;
            effectEvent.effectName=EditorGUILayout.TextField("特效资源名",effectEvent.effectName);
        }else if (mCurEvet is PlayAudioEvent)
        {
            PlayAudioEvent mAudioEvent = mCurEvet as PlayAudioEvent;
            mAudioEvent.audioName = EditorGUILayout.TextField("音效资源名", mAudioEvent.audioName);
        }

    }


    void DrawCreateUI()
    {
        GUILayout.Label("选择要创建的事件");
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        if (GUILayout.Button("播放特效"))
        {
            CreateEvent(new ShowEffectEvent());
        }

        if (GUILayout.Button("播放声音"))
        {
            CreateEvent(new PlayAudioEvent());
        }



        GUILayout.EndScrollView();
    }

    void CreateEvent(AIClipEvent clipEvent)
    {
        if (onCreate != null)
        {
            onCreate(clipEvent);
            //初始名为赋值为类型名
            clipEvent.eventName = clipEvent.ToString();
            mCurEvet = clipEvent;
            onCreate = null;
        }
        else
        {
            Debug.LogError("没有创建回调");
        }
    }


}
