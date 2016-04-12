using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Text;
public class AIDataEditor : EditorWindow {
    [MenuItem("AIFrame/Open/DataEditor #&o")]
    public static void OpenWindow()
    {
        EditorWindow.GetWindow<AIDataEditor>();
    }
    bool locked = false;

    private GUIStyle m_IconStyle = new GUIStyle();
 
    public static  List<UIAIDataUnit> listUIGroups = new List<UIAIDataUnit>();
    public static  AIDataSet aiDataSet = new AIDataSet();  //可编辑的所有AI集合
    public Vector2 groupScrollPos = new Vector2();
    public Vector2 conditionListPos = new Vector2();
    public Vector2 selectionScrollPos = new Vector2();

    private void OnEnable()
    {
        Texture2D icon = Resources.Load<Texture2D>("Icons/icon");
        m_IconStyle.normal.background = icon;
        title = "AIF编辑器";

    }
    void ShowButton(Rect rect)
    {
        locked = GUI.Toggle(rect, locked, GUIContent.none, "IN LockButton");
        rect.x -=position.width- 140.0f;
        GUI.Button(new Rect(rect.x, rect.y-10, 25, 25), GUIContent.none, m_IconStyle);
    }

    void Start()
    {
        LoadDataFromFile(true);
    }


    void OnGUI()
    {
        DrawMenus();
        DrawAIUnits();
        DrawSelectedAiClipOrGroup();
        HandleKeyboardShortCut();
    }

    void DrawMenus()
    {
        int menuWidth = 60;
        GUILayout.BeginHorizontal();
        
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
        if (GUILayout.Button("新建AI组", GUILayout.Width(60)))
        {
            AIDataUnitEditWnd wnd = EditorWindow.GetWindow<AIDataUnitEditWnd>();
            AIDataUnit unit = new AIDataUnit();
            wnd.SetData(AIDataUnitEditWnd.EditMode.Create, unit, delegate(AIDataUnit data)
            {
                CreateAIGroup(data);
            });
        }
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
            GUI.color = uiGroup == AIDataSelection.selectedUnit ? Color.yellow : prevColor;
            if(GUILayout.Button(uiGroup.Name,GUILayout.Width(200)))
            {
                AIDataSelection.SelectAIDataUnit(uiGroup);
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
            GUILayout.Space(20);
            bool expandGroup = groupUnit.GetChildExpand(groupIndex);
            if(GUILayout.Button(expandGroup?"-":"+",GUILayout.Width(25)))
            {
                groupUnit.SetChildExpand(groupIndex, !expandGroup);
            }
           
            if (AIFUIUtility.LayoutButtonWithColor(clipGroup.GroupName,clipGroup==AIDataSelection.selecteClipGroup?Color.magenta:GUI.color,170))
            {
                AIDataSelection.SelectAIGroup(clipGroup);
            }

            if (GUILayout.Button("Add", GUILayout.Width(45)))
            {
                AIClip aiClip = new AIClip();
                AIClipEditWnd wnd = EditorWindow.GetWindow<AIClipEditWnd>();
                wnd.SetData(AIClipEditWnd.EditMode.Create, clipGroup,aiClip, delegate
                {
                    clipGroup.aiClipList.Add(aiClip);
                });
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

            if (expandGroup)
            {
                const int indentSpace = 40;
                #region 绘制展开的AI组片断
                if (clipGroup.aiClipList.Count == 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(indentSpace);
                    GUILayout.Label("空");
                    GUILayout.EndHorizontal();
                }
                else
                {
                   
                    for (int i = 0; i < clipGroup.aiClipList.Count; i++)
                    {
                        AIClip clip = clipGroup.aiClipList[i];
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(indentSpace);
                        Color col = AIDataSelection.selectedAiClip == clip ? Color.green : GUI.color;
                        if (AIFUIUtility.LayoutButtonWithColor(clip.NameOnUI,col,AIFGUISKin.GetStyle("button"),GUILayout.Width(150)))
                        {
                            AIDataSelection.SelectAIDataUnit(groupUnit);
                            AIDataSelection.SelectAIClip(clip);
                        }
                        if (clip == AIDataSelection.selectedAiClip)
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
                #endregion
            } 
                
        }
    }

    void DrawSelectedAiClipOrGroup()
    {
        GUILayout.BeginArea(new Rect(410,0, position.width*0.6f, position.height*0.9f));
        //selectionScrollPos = GUILayout.BeginScrollView(selectionScrollPos, true, true, GUILayout.Width(400), GUILayout.Height(position.height * 0.9f));

        GUILayout.Label("当前选中");
        AIClip selectedClip =AIDataSelection.selectedAiClip;
        AIClipGroup selectedGroup = AIDataSelection.selecteClipGroup;
        if (selectedClip != null)
        {
            AIFUIUtility.DrawAIClip(selectedClip,selectedGroup);
            GUILayout.BeginHorizontal();
            GUILayout.Label("连接片断列表：", GUILayout.Width(100));
            if (GUILayout.Button("添加连接", GUILayout.Width(100)))
            {
                AILink link = new AILink();
                selectedClip.linkAIClipList.Add(link);
                AILinkEditWnd wnd = EditorWindow.GetWindow<AILinkEditWnd>();
                wnd.SetData(AIDataSelection.selecteClipGroup,link);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginArea(new Rect(250, 150, 500, 700));
            AIFUIUtility.DrawAiEvetList(selectedClip);
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(450, 150, 500, 700));
            AIFUIUtility.DrawHitDefinitionList(selectedClip);
            GUILayout.EndArea();

            #region Links
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
                    //string fullClipName=
                    if (AIFUIUtility.LayoutButtonWithColor(ai.linkToClip, AIDataSelection.IsSelectedLinkClip(ai) ? Color.cyan : Color.magenta, 150))
                    {
                        AIDataSelection.SelectLinkClip(ai);
                        AILinkEditWnd.EditAILink(selectedGroup, ai);
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




                #region 绘制选择连接
                //AILink link = curSelection.SelectedLink;
                //if (link != null)
                //{
                //    GUILayout.BeginArea(new Rect(300, 50, 500, 700));
                //    link.linkToClip = AIFUIUtility.DrawTextField(link.linkToClip, "连接目标");
                //    conditionListPos = GUILayout.BeginScrollView(conditionListPos, true, true, GUILayout.Width(300), GUILayout.Height(position.height * 0.6f));
                //    AIFUIUtility.DrawAiLinkConditions(link);
                //    GUILayout.EndScrollView();
                //    GUILayout.EndArea();
                //} 
                #endregion
            } 
            #endregion

           

        }
        else  //没选中AI片断就检测时候在编辑AI组
        {
            if (AIDataSelection.selecteClipGroup != null)
            {
                AIClipGroup clipGroup = AIDataSelection.selecteClipGroup;
                clipGroup.name = AIFUIUtility.DrawTextField(clipGroup.name,"Ai组名称");
                clipGroup.moveSpeed = EditorGUILayout.FloatField("移动速度", clipGroup.moveSpeed);
                clipGroup.targetType = (ETargetType)AIFUIUtility.DrawCustomEnum("目标类型", clipGroup.targetType, 100);
                AIFUIUtility.DrawAIShape(AIDataSelection.selecteClipGroup.shape);
                AIFUIUtility.DrawCommanAnimation(AIDataSelection.selecteClipGroup.commonAnimation, AIDataSelection.selecteClipGroup);
            }
            else if (AIDataSelection.selectedUnit != null)
            {
                AIDataSelection.selectedUnit.aiData.AiName = AIFUIUtility.DrawTextField(AIDataSelection.selectedUnit.aiData.AiName, "AI单位名称");
                //
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
        AIDataSelection.SelectAIDataUnit(uiGroup);
        Repaint();
    }

    void AddAIGroup(AIDataUnit aiData)
    {
        UIAIDataUnit uiGroup = new UIAIDataUnit(aiData);
        listUIGroups.Add(uiGroup);
        AIDataSelection.SelectAIDataUnit(uiGroup);
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
                //在保存之前备份一个文件， 下面保存出错的话就用备份数据来恢复
                string backUpFile = path + ".Backup";
                if (File.Exists(backUpFile))
                {
                    File.Delete(backUpFile);
                }
                File.Copy(path,backUpFile);

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
                Debug.LogError("保存数据错误,要恢复数据请用备份数据" + ex.Message);
                EditorUtility.DisplayDialog("警告", "数据保存失败，请用备份文件恢复", "OK");


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

    /// <summary>
    /// 检查是否可以创建这个新的数据（ID 是否相同等等）
    /// </summary>
    /// <param name="dataToCreate"></param>
    /// <returns></returns>
    public static bool CheckCreateNew(AIClipGroup clipGroup,AIClip clip)
    {
        if (string.IsNullOrEmpty(clip.clipKey))
        {
            EditorUtility.DisplayDialog("提示", "片断名不能为空", "好吧V_V");
            return false;
        }

        AIClip srcClip= clipGroup.aiClipList.Find(delegate(AIClip targetClip)
        {
            return targetClip.clipKey == clip.clipKey;
        });

        if (srcClip != null)
        {
            EditorUtility.DisplayDialog("提示", "动画片断键值和已有的重复！", "确定");
            return false;
        }
        return true;
    }

    void HandleKeyboardShortCut()
    {
        if (EditorWindow.focusedWindow != this)
        {
            return;
        }

        
        Event curEvent = Event.current;
        if (curEvent.isKey)
        {
            //这里为了和内置的Ctrl C  CtrlV 冲突，我们用Shift
            if (curEvent.keyCode == KeyCode.C && curEvent.shift) //复制
            {
                AIDataSelection.CopySelection();
            }
            else if (curEvent.keyCode == KeyCode.V && curEvent.shift) //粘贴
            {
                AIDataSelection.PasteCopyData();
            }
            
        }
        
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
    /// <summary>
    /// 记录所有子对象的展开状态
    /// </summary>
    private List<bool> ChildExpandList = new List<bool>();

    public bool GetChildExpand(int childIndex)
    {
        MakeSureListEnough(childIndex);
        return ChildExpandList[childIndex];
    }

    public void  SetChildExpand(int childIndex,bool expand)
    {
        MakeSureListEnough(childIndex);
        ChildExpandList[childIndex] = expand;
    }

    /// <summary>
    /// 确认状态列表有足够的长度可以使用目标索引，不够就新建在后面
    /// </summary>
    /// <param name="targetIndex"></param>
    void MakeSureListEnough(int targetIndex)
    {
        while (ChildExpandList.Count < targetIndex + 1)
        {
            ChildExpandList.Add(false);
        }
    }

    public bool bExpand;
    public AIDataUnit aiData;
}

/// <summary>
/// 记录当前选中数据
/// </summary>
public static class AIDataSelection
{
    public static  UIAIDataUnit selectedUnit;
    public static   AIClip selectedAiClip;
    public static AIDataUnit selectedDataUnit;
    public static AIClipGroup selecteClipGroup;
    public static AiClipHitData selectedHitData;
    public static AIClipEvent aiEvent;
    /// <summary>
    /// 和选中AI组中选中AI片断连接的片断
    /// </summary>
    private static  AILink mSelectedLink;

    public static AILink SelectedLink
    {
        get { return mSelectedLink; }
    }

    public static  void SelectAIDataUnit(UIAIDataUnit groupUI)
    {
        if (selectedUnit != groupUI)
        {
            selectedUnit = groupUI;
            selectedDataUnit = groupUI.aiData;
          
        }
        selectedAiClip = null;
        mSelectedLink = null;
        selecteClipGroup = null;
    }

    public static void SelectAIGroup(AIClipGroup group)
    {
        if (selecteClipGroup != group)
        {
            selecteClipGroup = group;
            
        }
        selectedAiClip = null;
        mSelectedLink = null;
    }


    public static void SelectAIClip(AIClip aiClip)
    {
        if (selectedAiClip != aiClip)
        {
            selectedAiClip = aiClip;
            //要保证选择AI片断后自动选择这个片断所在的AI组
            selecteClipGroup = FindOwnerClipGroup(aiClip);
            mSelectedLink = null;
            selectedHitData = null;
            aiEvent = null;
        }
    }

    public static void SelectLinkClip(AILink link)
    {
        if (link != mSelectedLink)
        {
            if (selectedAiClip.linkAIClipList.Contains(link))
            {
                mSelectedLink = link;
            }
            else
            {
                Debug.LogError("此片断不在选择片断的连接列表");
            }
        }
    }

    public static void SelectedHitData(AiClipHitData hitData)
    {
        selectedHitData = hitData;
    }

    public static void SelectedAIEvent(AIClipEvent clipEvent)
    {
        aiEvent = clipEvent;
    }

    public static  bool IsSelectedLinkClip(AILink clip)
    {
        return mSelectedLink != null && mSelectedLink == clip;
    }

    /// <summary>
    /// 寻找制定AI片断归属的AI组
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public static AIClipGroup FindOwnerClipGroup(AIClip clip)
    {
        for (int i = 0; i < AIDataEditor.aiDataSet.aiDataList.Count; i++)
        {
            AIDataUnit aiUnit = AIDataEditor.aiDataSet.aiDataList[i];
            AIClipGroup clipGroup = aiUnit.aiGroups.Find(delegate(AIClipGroup Group)
            {
                return Group.aiClipList.Contains(clip);
            });
            if (clipGroup != null)
            {
                return clipGroup;
            }
        }
        Debug.LogError(string.Format("片断{0}没有找到归属组", clip));
        return null;
    }

    /// <summary>
    /// 复制选中
    /// </summary>
    public static void CopySelection()
    {
        if (selectedAiClip != null)
        {
            CustomClipBoard.CopyData(selectedAiClip);
        }else if (selecteClipGroup != null)
        {
            CustomClipBoard.CopyData(selecteClipGroup);
        }else if (selectedDataUnit != null)
        {
            CustomClipBoard.CopyData(selectedDataUnit);
        }
    }

    /// <summary>
    /// 粘贴之前复制的数据到当前选择
    /// </summary>
    public static void PasteCopyData()
    {
        object clipBoardObj;
        CustomClipBoard.GetCopyObject(out clipBoardObj);
        if (clipBoardObj == null)
        {
            Debug.LogError("剪切板没有内容可以粘贴");
        }
        else
        {
            clipBoardObj = Utility.XmlDeepCloneObject(clipBoardObj);

            if (clipBoardObj is AIClip && selecteClipGroup!=null)
            {
                selecteClipGroup.aiClipList.Add((AIClip)clipBoardObj);
            }else if (clipBoardObj is AIClipGroup && selectedDataUnit != null)
            {
                selectedDataUnit.aiGroups.Add((AIClipGroup) clipBoardObj);
            }
            else
            {
                Debug.LogError("没有合适的粘贴位置");
            }
        }
    }


}