using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Chat_C2S : ProtoBase_C2S
{
    public int userId;
    public string text;

    public Chat_C2S()
    {
        protoCode = 10000;
    }
   
    public override List<byte> Encode()
    {
        WriteInt(protoCode);
        WriteInt(userId);
        WriteString(text);
        
        return listBytes;
    }

    public void Decode(byte[] srcBytes)
    {
        mBytes = srcBytes;
        ReadInt32(out protoCode);
        ReadInt32(out userId);
        ReadString(out text);
    }

    public override string ToString()
    {
        return ("User id " + userId + " name " + text);
    }
}
