using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Text;
public class AIDataEditor : EditorWindow {
    [MenuItem("AIFrame/Open/DataEditor")]
    public static void OpenWindow()
    {
        EditorWindow.GetWindow<AIDataEditor>();
    }
    
    public static  List<AIGroupUnit> listUIGroups = new List<AIGroupUnit>();
    static  AIDataSet aiDataSet = new AIDataSet();  //可编辑的所有AI集合
    public Vector2 groupScrollPos = new Vector2();
    public Vector2 conditionListPos = new Vector2();
    public AIDataSelection curSelection=new AIDataSelection();
    void Start()
    {
        LoadDataFromFile(true);
    }


    void OnGUI()
    {
        DrawMenus();
        DrawAIClipGroups();
        DrawSelectedAiClipOrGroup();
    }

    void DrawMenus()
    {
        int menuWidth = 60;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("新建AI组", GUILayout.Width(menuWidth)))
        {
            AIClipGroup clipGroup = new AIClipGroup();
            CreateAIGroup(clipGroup);
        }
        if (GUILayout.Button("导入", GUILayout.Width(menuWidth)))
        {
            LoadDataFromFile(true);
        }
        if (GUILayout.Button("选择导入", GUILayout.Width(menuWidth)))
        {
            LoadDataFromFile(false);
        }
        if (GUILayout.Button("保存", GUILayout.Width(menuWidth)))
        {
            SaveDataToFile(true);
        }
        if (GUILayout.Button("另存为", GUILayout.Width(menuWidth)))
        {
            SaveDataToFile(false);
        }
        if (GUILayout.Button("清空界面", GUILayout.Width(menuWidth)))
        {
            aiDataSet.aiGroups.Clear();
            listUIGroups.Clear();
            return;
        }

        GUILayout.EndHorizontal();
    }

   

    /// <summary>
    /// 绘制所有的AI组
    /// </summary>
    void DrawAIClipGroups()
    {
        groupScrollPos = GUILayout.BeginScrollView(groupScrollPos, false, true, GUILayout.Width(400), GUILayout.Height(position.height * 0.9f));
        for (int i = 0; i < listUIGroups.Count; i++)
        {
            AIGroupUnit uiGroup = listUIGroups[i];
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(uiGroup.bExpand ? "-" : "+",GUILayout.Width(20)))
            {
                uiGroup.bExpand =! uiGroup.bExpand;
            }
            Color prevColor = GUI.color;
            GUI.color = uiGroup == curSelection.selectedGroup ? Color.yellow : prevColor;
            if(GUILayout.Button(uiGroup.Name,GUILayout.Width(200)))
            {
                curSelection.SelectAIGroup(uiGroup);
            }
            GUI.color = prevColor;
            if (GUILayout.Button("添加", GUILayout.Width(40)))
            {
                AIClip clip = new AIClip();
                uiGroup.aiData.aiClipList.Add(clip);
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                if (EditorUtility.DisplayDialog("提示", "确定要AI组吗？", "确定"))
                {
                    DeleteAIGroup(uiGroup);
                    return;
                }
            }

           

            GUILayout.EndHorizontal();
            if (uiGroup.bExpand)//没有展开
            {
                DrawAIClipGroup(uiGroup);
            }
        }
        GUILayout.EndScrollView();
    }

    void DrawAIClipGroup(AIGroupUnit groupUnit)
    {
        AIClipGroup clipGroup = groupUnit.aiData;
        if (clipGroup.aiClipList.Count == 0)
        {
            GUILayout.Label("空");
        }
        else
        {
            for (int i = 0; i < clipGroup.aiClipList.Count; i++)
            {
                AIClip clip = clipGroup.aiClipList[i];
                GUILayout.BeginHorizontal();
                if (AIFUIUtility.LayoutButtonWithColor(clip.name, curSelection.selectedAiClip == clip ? Color.green : GUI.color, 150))
                {
                    curSelection.SelectAIGroup(groupUnit);
                    curSelection.SelectAIClip(clip);
                }
                if (clip == curSelection.selectedAiClip)
                {
                }
                if(GUILayout.Button("X",GUILayout.Width(30)))
                {
                    if (EditorUtility.DisplayDialog("删除AI片断", "确定要删除?", "Yes"))
                    {
                        DeleteAiClipFromAiGroup(clip, clipGroup);
                        clipGroup.aiClipList.Remove(clip);
                        return;
                    }
                }
                GUILayout.EndHorizontal();


            }
        }
    }

    void DrawSelectedAiClipOrGroup()
    {
        GUILayout.BeginArea(new Rect(position.width*0.4f,0, position.width*0.6f, position.height*0.9f));
        GUILayout.Label("当前选中");
        AIClip selectedClip = curSelection.selectedAiClip;
        if (selectedClip != null)
        {
            selectedClip.name = AIFUIUtility.DrawTextField(selectedClip.name, "片断名称", 200);
            selectedClip.animationName = AIFUIUtility.DrawTextField(selectedClip.animationName, "动画名称", 200);
            selectedClip.attackRange = EditorGUILayout.FloatField(selectedClip.attackRange, GUILayout.Width(90));
            GUILayout.BeginHorizontal();
            GUILayout.Label("连接片断列表：", GUILayout.Width(100));
            if (GUILayout.Button("添加连接", GUILayout.Width(100)))
            {
                AIClipSelectWnd wnd = EditorWindow.GetWindow<AIClipSelectWnd>();
                wnd.SetGroupData(curSelection.selectedGroup.aiData, selectedClip);
            }
            GUILayout.EndHorizontal();
            if (selectedClip.linkAIClipList.Count == 0)
            {
                GUILayout.Label("空列表", GUILayout.Width(300));
            }
            else
            {
                for (int i = 0; i < selectedClip.linkAIClipList.Count; i++)
                {
                    AILinkClip aiClip = selectedClip.linkAIClipList[i];
                    GUILayout.BeginHorizontal();
                    if (AIFUIUtility.LayoutButtonWithColor(aiClip.linkToClip.name,curSelection.IsSelectedLinkClip(aiClip)?Color.cyan:Color.magenta,250))
                    {
                        curSelection.SelectLinkClip(aiClip);
                    }
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("提示", "确定要删除连接吗", "确定"))
                        {
                            selectedClip.linkAIClipList.Remove(aiClip);
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                }

                AILinkClip linkClip = curSelection.SelectedLinkClip;
                if (linkClip != null)
                {
                    GUILayout.BeginArea(new Rect(300, 50, 500, 700));
                    conditionListPos = GUILayout.BeginScrollView(conditionListPos,true, true, GUILayout.Width(300), GUILayout.Height(position.height * 0.6f));
                    AIFUIUtility.DrawAiLinkConditions(linkClip);
                    GUILayout.EndScrollView();
                    GUILayout.EndArea();
                }
            }
        }
        else  //没选中AI片断就检测时候在编辑AI组
        {
            if (curSelection.selectedGroup != null)
            {
                curSelection.selectedGroup.aiData.name = AIFUIUtility.DrawTextField(curSelection.selectedGroup.aiData.name, "AI组名称");
                AIFUIUtility.DrawAIShape(curSelection.selectedGroup.aiData.shape);
            }
        }



        GUILayout.EndArea();
    }


    void DeleteAIGroup(AIGroupUnit deleteTarget)
    {
        if (aiDataSet.aiGroups.Contains(deleteTarget.aiData))
        {
            aiDataSet.aiGroups.Remove(deleteTarget.aiData);
        }
        listUIGroups.Remove(deleteTarget);
        Repaint();
    }

    /// <summary>
    /// 从AI组里面删除一个片断， 并且会删除其他片断对这个片断的连接
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="clipGroup"></param>
    void DeleteAiClipFromAiGroup(AIClip clip, AIClipGroup clipGroup)
    {
        if (clipGroup.aiClipList.Contains(clip))
        {
            for (int i = 0; i < clipGroup.aiClipList.Count; i++)
            {
                AIClip aiClip = clipGroup.aiClipList[i];
                aiClip.linkAIClipList.RemoveAll(delegate(AILinkClip targetClip)
                {
                    return targetClip.linkToClip == clip;
                });
            }
        }
        else
        {
            Debug.LogError("片断" + clip + "不在片断组里面"+clipGroup);
        }
    }

    /// <summary>
    /// 创建一个新的AI组  并且添加到UI列表里面
    /// </summary>
    /// <param name="group"></param>
    void CreateAIGroup(AIClipGroup group)
    {
        aiDataSet.aiGroups.Add(group);
        AIGroupUnit uiGroup = new AIGroupUnit(group);
        listUIGroups.Add(uiGroup);
        curSelection.SelectAIGroup(uiGroup);
    }

    void AddAIGroup(AIClipGroup group)
    {
        AIGroupUnit uiGroup = new AIGroupUnit(group);
        listUIGroups.Add(uiGroup);
        curSelection.SelectAIGroup(uiGroup);
    }

   

    private void LoadDataFromFile(bool useDefaultPath)
    {
        string path = "";
        if (useDefaultPath)
            path = DefaultFilePath ;
        else
           path = EditorUtility.OpenFilePanel("导入数据", DefaultFilePath, "*.*");
       
        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AIDataSet));
                FileStream stream = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(stream);
                aiDataSet = (AIDataSet)serializer.Deserialize(reader);
                stream.Flush();
                listUIGroups.Clear();
                for (int i = 0; i < aiDataSet.aiGroups.Count; i++)
                {
                    AddAIGroup(aiDataSet.aiGroups[i]);
                }
                stream.Close();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("加载数据错误" + ex.Message);
            }
        }
    }

    private void SaveDataToFile(bool useDefaultPath=true)
    {
        string path = "";
        if(useDefaultPath)
             path =DefaultFilePath;
        else
            path = EditorUtility.SaveFilePanel("保存数据", DefaultFilePath, "AIDataSet.xml", "*.xml");

        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                using (StreamWriter output =
                    new StreamWriter(new FileStream(path, FileMode.Create), Encoding.Unicode))
                {
                    using (XmlWriter xmlWriter =
                        XmlWriter.Create(output, new XmlWriterSettings()))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AIDataSet));
                        
                        serializer.Serialize(xmlWriter, aiDataSet);
                       
                        Debug.Log("保存成功" + path + System.DateTime.Now.ToLongTimeString());
                    }
                }

               
            }
            catch (System.Exception ex)
            {
                Debug.LogError("保存数据错误" + ex.Message);
            }
        }
        else
        {
            Debug.LogError("保存路径为空");
        }
    }

    public string DefaultFilePath
    {
        get { return Application.dataPath + "/../AIDataSet.xml"; }
    }

}

public class AIGroupUnit
{
    public AIGroupUnit(AIClipGroup data)
    {
        aiData = data;
    }

    public string Name
    {
        get { return aiData.GroupName; }
    }
    
    public bool bExpand;
    public AIClipGroup aiData;
}

/// <summary>
/// 记录当前选中数据
/// </summary>
public class AIDataSelection
{
    public  AIGroupUnit selectedGroup;
    public  AIClip selectedAiClip;
    /// <summary>
    /// 和选中AI组中选中AI片断连接的片断
    /// </summary>
    private AILinkClip mSelectedLinkClip;

    public AILinkClip SelectedLinkClip
    {
        get { return mSelectedLinkClip; }
    }

    public void SelectAIGroup(AIGroupUnit groupUI)
    {
        if (selectedGroup != groupUI)
        {
            selectedGroup = groupUI;
            selectedAiClip = null;
            mSelectedLinkClip = null;
        }
    }

    public void SelectAIClip(AIClip aiClip)
    {
        if (selectedAiClip != aiClip)
        {
            selectedAiClip = aiClip;
            //要保证选择AI片断后自动选择这个片断所在的AI组
            if (selectedGroup == null || selectedGroup.aiData.aiClipList.Contains(aiClip) == false)
            {
                selectedGroup = null;
                for (int i = 0; i < AIDataEditor.listUIGroups.Count; i++)
                {
                    if (AIDataEditor.listUIGroups[i].aiData.aiClipList.Contains(aiClip))
                    {
                        selectedGroup = AIDataEditor.listUIGroups[i];
                        break;
                    }
                }
                if (selectedGroup == null)
                {
                    Debug.LogError("AI片断没有找到归宿的组" + aiClip);
                }
            }
            mSelectedLinkClip = null;
        }
    }

    public void SelectLinkClip(AILinkClip linkClip)
    {
        if (linkClip != mSelectedLinkClip)
        {
            if (selectedAiClip.linkAIClipList.Contains(linkClip))
            {
                mSelectedLinkClip = linkClip;
            }
            else
            {
                Debug.LogError("此片断不在选择片断的连接列表");
            }
        }
    }

    public bool IsSelectedLinkClip(AILinkClip clip)
    {
        return mSelectedLinkClip != null && mSelectedLinkClip == clip;
    }
    
}