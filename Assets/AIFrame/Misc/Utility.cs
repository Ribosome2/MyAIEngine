using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static object DeepCloneObject(object srcObj)
    {
        using (Stream objectStream = new MemoryStream())
        {
            //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, srcObj);
            objectStream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(objectStream);
        }   
    }

    public static object XmlDeepCloneObject(object srcObj)
    {
        using (Stream stream = new MemoryStream())
        {
            XmlSerializer serializer = new XmlSerializer(srcObj.GetType());
            serializer.Serialize(stream, srcObj);
            stream.Seek(0, SeekOrigin.Begin);
            return serializer.Deserialize(stream);
        }  
    }


}
