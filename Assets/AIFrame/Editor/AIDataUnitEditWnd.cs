using UnityEngine;
using System.Collections;
using UnityEditor;
public class AIDataUnitEditWnd :EditorWindow
{
    public delegate void OnCreateNew(AIDataUnit data);

    public OnCreateNew onCreateNew;
    public enum EditMode
    {
        Create,
        Editing,
    }
    private AIDataUnit mDataUnit;
    private EditMode mMode;
    public void SetData(EditMode editMode, AIDataUnit dataToEdit,OnCreateNew createCallBack)
    {
        mDataUnit = dataToEdit;
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
                    if (AIDataEditor.CheckCreateNew(mDataUnit))
                    {
                        if (onCreateNew != null)
                        {
                            onCreateNew(mDataUnit);
                            Close();
                        }
                    }
                }
            }

            mDataUnit.Id = EditorGUILayout.IntField("Id", mDataUnit.Id);
            mDataUnit.AiName = AIFUIUtility.DrawTextField(mDataUnit.AiName, "AiName", 100);
        }
    }

}
