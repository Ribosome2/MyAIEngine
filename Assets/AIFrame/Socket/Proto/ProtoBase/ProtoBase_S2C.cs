using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 服务器下发 协议抽象基类
/// </summary>
public abstract class ProtoBase_S2C:ProtoBase
{
    /// 解码
    /// </summary>
    public abstract void Decode(byte[] srcBytes);
}
