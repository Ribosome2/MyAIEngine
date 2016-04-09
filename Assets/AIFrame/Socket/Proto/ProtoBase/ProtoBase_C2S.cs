using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 客户端像服务器发送的协议抽象基类
/// </summary>
public abstract class ProtoBase_C2S:ProtoBase  {

  /// <summary>
  /// 注意这里返回的字节数组只是协议body的字节内容，在发送的时候要在前面加Header来标记协议的长度
  /// </summary>
  /// <returns></returns>
    public abstract List<byte> Encode();

    protected void BeginEncode()
    {
        listBytes.Clear();
    }
}
