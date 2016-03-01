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
    
    public static  List<UIAIDataUnit> listUIGroups = new List<UIAIDataUnit>();
    public static  AIDataSet aiDataSet = new AIDataSet();  //可编辑的所有AI集合
    public Vector2 groupScrollPos = new Vector2();
    public Vector2 conditionListPos = new Vector2();
    public Vector2 selectionScrollPos = new Vector2();
    public AIDataSelection curSelection=new AIDataSelection();
    
    void Start()
    {
        LoadDataFromFile(true);
    }


    void OnGUI()
    {
        DrawMenus();
        DrawAIUnits();
        DrawSelectedAiClipOrGroup();
    }

    void DrawMenus()
    {
        int menuWidth = 60;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("新建AI组", GUILayout.Width(menuWidth)))
        {
            AIDataUnitEditWnd wnd = EditorWindow.GetWindow<AIDataUnitEditWnd>();
            AIDataUnit unit= new AIDataUnit();
            wnd.SetData(AIDataUnitEditWnd.EditMode.Create, unit, delegate(AIDataUnit data)
            {
                CreateAIGroup(data);
            });
            
           
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
            aiDataSet.aiDataList.Clear();
            listUIGroups.Clear();
            return;
        }

        GUILayout.EndHorizontal();
    }

   

    /// <summary>
    /// 绘制所有的AI组
    /// </summary>
    void DrawAIUnits()
    {
        groupScrollPos = GUILayout.BeginScrollView(groupScrollPos, false, true, GUILayout.Width(400), GUILayout.Height(position.height * 0.9f));
        for (int i = 0; i < listUIGroups.Count; i++)
        {
            UIAIDataUnit uiGroup = listUIGroups[i];
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(uiGroup.bExpand ? "-" : "+",GUILayout.Width(20)))
            {
                uiGroup.bExpand =! uiGroup.bExpand;
            }
            Color prevColor = GUI.color;
            GUI.color = uiGroup == curSelection.selectedUnit ? Color.yellow : prevColor;
            if(GUILayout.Button(uiGroup.Name,GUILayout.Width(200)))
            {
                curSelection.SelectAIDataUnit(uiGroup);
            }
            GUI.color = prevColor;
            if (GUILayout.Button("添加", GUILayout.Width(40)))
            {
                AIClipGroup group = new AIClipGroup();
                uiGroup.aiData.aiGroups.Add(group);
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
                DrawAIUnit(uiGroup);
            }
        }
        GUILayout.EndScrollView();
    }

    void DrawAIUnit(UIAIDataUnit groupUnit)
    {
        AIDataUnit aiData = groupUnit.aiData;
        for (int groupIndex = 0; groupIndex < aiData.aiGroups.Count; groupIndex++)
        {
            AIClipGroup clipGroup = aiData.aiGroups[groupIndex];
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(clipGroup.GroupName, GUI.skin.FindStyle("sv_label_1"), GUILayout.Width(170)))
            {
                curSelection.SelectAIGroup(clipGroup);
            }
            if (GUILayout.Button("Add", GUILayout.Width(45)))
            {
                AIClip aiClip = new AIClip();
                clipGroup.aiClipList.Add(aiClip);
            }
            if (GUILayout.Button("X", GUILayout.Width(45)))
            {
                if(EditorUtility.DisplayDialog("警告","确定删除动作组?","确定"))
                {
                    aiData.aiGroups.Remove(clipGroup);
                    Repaint();
                    return;
                }
            }

            GUILayout.EndHorizontal();
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
                    if (AIFUIUtility.LayoutButtonWithColor(clip.name,
                        curSelection.selectedAiClip == clip ? Color.green : GUI.color, 150))
                    {
                        curSelection.SelectAIDataUnit(groupUnit);
                        curSelection.SelectAIClip(clip);
                    }
                    if (clip == curSelection.selectedAiClip)
                    {
                    }
                    if (GUILayout.Button("X", GUILayout.Width(30)))
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
    }

    void DrawSelectedAiClipOrGroup()
    {
        GUILayout.BeginArea(new Rect(position.width*0.4f,0, position.width*0.6f, position.height*0.9f));
        //selectionScrollPos = GUILayout.BeginScrollView(selectionScrollPos, true, true, GUILayout.Width(400), GUILayout.Height(position.height * 0.9f));

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
                AILink link = new AILink();
                selectedClip.linkAIClipList.Add(link);
                AILinkEditWnd wnd = EditorWindow.GetWindow<AILinkEditWnd>();
                wnd.SetData(curSelection.selecteClipGroup,link);
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
                    AILink ai = selectedClip.linkAIClipList[i];
                    GUILayout.BeginHorizontal();
                    if (AIFUIUtility.LayoutButtonWithColor(ai.linkToClip,curSelection.IsSelectedLinkClip(ai)?Color.cyan:Color.magenta,250))
                    {
                        curSelection.SelectLinkClip(ai);
                    }
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("提示", "确定要删除连接吗", "确定"))
                        {
                            selectedClip.linkAIClipList.Remove(ai);
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                }

                AILink link = curSelection.SelectedLink;
                if (link != null)
                {
                    GUILayout.BeginArea(new Rect(300, 50, 500, 700));
                    conditionListPos = GUILayout.BeginScrollView(conditionListPos,true, true, GUILayout.Width(300), GUILayout.Height(position.height * 0.6f));
                    AIFUIUtility.DrawAiLinkConditions(link);
                    GUILayout.EndScrollView();
                    GUILayout.EndArea();
                }
            }
        }
        else  //没选中AI片断就检测时候在编辑AI组
        {
            if (curSelection.selectedUnit != null)
            {
                curSelection.selectedUnit.aiData.AiName = AIFUIUtility.DrawTextField(curSelection.selectedUnit.aiData.AiName, "AI组名称");
                //AIFUIUtility.DrawAIShape(curSelection.selectedUnit.aiData..shape);
            }
        }


        //GUILayout.EndScrollView();
        GUILayout.EndArea();
       
    }


    void DeleteAIGroup(UIAIDataUnit deleteTarget)
    {
        if (aiDataSet.aiDataList.Contains(deleteTarget.aiData))
        {
            aiDataSet.aiDataList.Remove(deleteTarget.aiData);
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
            clipGroup.aiClipList.Remove(clip);
        }
        else
        {
            Debug.LogError("片断" + clip + "不在片断组里面"+clipGroup);
        }
    }

    /// <summary>
    /// 创建一个新的AI组  并且添加到UI列表里面
    /// </summary>
    /// <param name="unit"></param>
    void CreateAIGroup(AIDataUnit unit)
    {
        aiDataSet.aiDataList.Add(unit);
        UIAIDataUnit uiGroup = new UIAIDataUnit(unit);
        listUIGroups.Add(uiGroup);
        curSelection.SelectAIDataUnit(uiGroup);
        Repaint();
    }

    void AddAIGroup(AIDataUnit aiData)
    {
        UIAIDataUnit uiGroup = new UIAIDataUnit(aiData);
        listUIGroups.Add(uiGroup);
        curSelection.SelectAIDataUnit(uiGroup);
        Repaint();
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
                for (int i = 0; i < aiDataSet.aiDataList.Count; i++)
                {
                    AddAIGroup(aiDataSet.aiDataList[i]);
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
    /// <summary>
    /// 检查是否可以创建这个新的数据（ID 是否相同等等）
    /// </summary>
    /// <param name="dataToCreate"></param>
    /// <returns></returns>
    public static bool CheckCreateNew(AIDataUnit dataToCreate)
    {
        if(dataToCreate.Id==0)
        {
            EditorUtility.DisplayDialog("提示", "ID 不能为0", "好吧V_V");
            return false;
        }
        
        AIDataUnit src = AIDataEditor.aiDataSet.aiDataList.Find(delegate(AIDataUnit unit)
        {
            return unit.Id ==
                   dataToCreate.Id;
        });
        return src == null;
    }
}

public class UIAIDataUnit
{
    public UIAIDataUnit(AIDataUnit data)
    {
        aiData = data;
    }

    public string Name
    {
        get { return aiData.Id+"/"+aiData.AiName; }
    }
    
    public bool bExpand;
    public AIDataUnit aiData;
}

/// <summary>
/// 记录当前选中数据
/// </summary>
public class AIDataSelection
{
    public  UIAIDataUnit selectedUnit;
    public  AIClip selectedAiClip;
    public AIDataUnit selectedDataUnit;
    public AIClipGroup selecteClipGroup;
    /// <summary>
    /// 和选中AI组中选中AI片断连接的片断
    /// </summary>
    private AILink _mSelectedLink;

    public AILink SelectedLink
    {
        get { return _mSelectedLink; }
    }

    public void SelectAIDataUnit(UIAIDataUnit groupUI)
    {
        if (selectedUnit != groupUI)
        {
            selectedUnit = groupUI;
            selectedAiClip = null;
            _mSelectedLink = null;
        }
    }

    public void SelectAIGroup(AIClipGroup group)
    {
        if (selecteClipGroup != group)
        {
            selecteClipGroup = group;
            selectedAiClip = null;
            _mSelectedLink = null;
        }
    }


    public void SelectAIClip(AIClip aiClip)
    {
        if (selectedAiClip != aiClip)
        {
            selectedAiClip = aiClip;
            //要保证选择AI片断后自动选择这个片断所在的AI组
          
            _mSelectedLink = null;
        }
    }

    public void SelectLinkClip(AILink link)
    {
        if (link != _mSelectedLink)
        {
            if (selectedAiClip.linkAIClipList.Contains(link))
            {
                _mSelectedLink = link;
            }
            else
            {
                Debug.LogError("此片断不在选择片断的连接列表");
            }
        }
    }

    public bool IsSelectedLinkClip(AILink clip)
    {
        return _mSelectedLink != null && _mSelectedLink == clip;
    }

    
}