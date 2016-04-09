using UnityEngine;
using System.Collections;

public class Chat_S2C : ProtoBase_S2C
{
    public int userId;
    public string text;

    public override void Decode(byte[] srcBytes)
    {
        mBytes = srcBytes;
        ReadInt32(out protoCode);
        ReadInt32(out userId);
        ReadString(out text);
    }

}
