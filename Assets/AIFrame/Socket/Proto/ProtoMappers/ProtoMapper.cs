using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 协议映射工具类，用来指定什么协议由什么类来进行解析
/// </summary>
public partial class ProtoMapper
{
    private static Dictionary<int, Type> mProtoDictionary = new Dictionary<int, Type>();


    public static void Initilize()
    {
        AddProtoToMap(10000, typeof (Chat_S2C));
        AddProtoToMap(10001, typeof(Chat_S2C));
    }

    /// <summary>
    /// 在映射里面添加协议号对应的解析类
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="decoder"></param>
    static void AddProtoToMap(int protoCode, Type decoderType)
    {
 
        if (decoderType.BaseType != typeof(ProtoBase_S2C))
        {
            Debug.LogError(decoderType + "is not a valid decoder");
        }

        if (mProtoDictionary.ContainsKey(protoCode))
        {
            Debug.LogError("Proto " + protoCode + " already map to decoder " + mProtoDictionary[protoCode]);
        }
        else
        {
            mProtoDictionary.Add(protoCode,decoderType);
        }
    }

    public static ProtoBase_S2C GetProtoDecoder(int protoCode)
    {
        if (!mProtoDictionary.ContainsKey(protoCode))
        {
            Debug.LogError("Proto " + protoCode + " doesn't have any decoder ");
            return null;
        }
        else
        {
            Type decoderType=mProtoDictionary[protoCode];
            
            ProtoBase_S2C decoder = null;
            decoder = decoderType.Assembly.CreateInstance(decoderType.Name) as ProtoBase_S2C;
            
            return  decoder;
        }
    }


}
