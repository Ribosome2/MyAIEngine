using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class AIDataMgr:Singleton<AIDataMgr>
{
    private AIDataSet mDataSet;

    AIDataSet dataSet
    {
        get
        {
            if (mDataSet == null)
            {
                mDataSet=LoadDataFromFile();
            }
            return mDataSet;
        }
    }


    public AIDataUnit GetAIUnitData(int aiID)
    {
         AIDataUnit targetData= dataSet.aiDataList.Find(delegate(AIDataUnit dataUnit)
        {
            return dataUnit.Id == aiID;
        });

        if (targetData == null)
        {
            Debug.LogError(string.Format("找不到AI数据{0}", aiID));
        }
        return targetData;
    }


    private AIDataSet LoadDataFromFile()
    {
        AIDataSet data = null;
        string path = "";
        
        path = DefaultFilePath;

        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AIDataSet));
                FileStream stream = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(stream);
                data = (AIDataSet)serializer.Deserialize(reader);
               
                stream.Close();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("加载数据错误" + ex.Message);
            }
        }

        return data;
    }

   

    public string DefaultFilePath
    {
        get { return Application.dataPath + "/../AIDataSet.xml"; }
    }
}
