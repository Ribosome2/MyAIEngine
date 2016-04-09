using UnityEngine;
using System.Collections;

public class GetUserInfo_S2C : ProtoBase_S2C
{
    public int userId;
    public string name;
    public int age;
    public override void Decode(byte[] srcBytes)
    {
        mBytes = srcBytes;
        ReadInt32(out protoCode);
        ReadInt32(out userId);
        ReadString(out name);
        ReadInt32(out age);
    }

}
