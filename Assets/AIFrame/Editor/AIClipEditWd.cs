using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class AIClipEditWnd :EditorWindow
{
    public delegate void OnCreateNew(AIClip data);

    public OnCreateNew onCreateNew;
    public enum EditMode
    {
        Create,
        Editing,
    }
    private AIClip mDataUnit;
    private AIClipGroup mClipGroup;
    private EditMode mMode;
    public void SetData(EditMode editMode,AIClipGroup clipGroup, AIClip dataToEdit, OnCreateNew createCallBack)
    {
        mDataUnit = dataToEdit;
        mClipGroup = clipGroup;
        mMode = editMode;
        onCreateNew = createCallBack;
    }

    void OnGUI()
    {
        if (mDataUnit != null)
        {
            if (mMode == EditMode.Create)
            {
                if (GUILayout.Button("Save", GUILayout.Width(80)))
                {
                    if (AIDataEditor.CheckCreateNew(mClipGroup,mDataUnit))
                    {
                        if (onCreateNew != null)
                        {
                            onCreateNew(mDataUnit);
                            Close();
                        }
                    }
                }
            }
            mDataUnit.animationName = AIFUIUtility.DrawTextField(mDataUnit.animationName, "动画片断名", 100);
            mDataUnit.name = AIFUIUtility.DrawTextField(mDataUnit.name, "名字");
            
        }
    }

    

}
