using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GetUserInfo_C2S : ProtoBase_C2S
{
    public int userId;
    public string name;
    public int age;
    public GetUserInfo_C2S()
    {
        protoCode = 10001;
    }
   
    public override List<byte> Encode()
    {
        WriteInt(protoCode);
        WriteInt(userId);
        WriteString(name);
        WriteInt(age);
        
        return listBytes;
    }


    public override string ToString()
    {
        return ("User id " + userId + " name " + name +" age "+age);
    }
}
