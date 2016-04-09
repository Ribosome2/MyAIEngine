//This file is generated by code, DO NOT EDIT !!!! 
using UnityEngine;
public class roleInfo: TableData
{
    public int  ID;
    public string  name;
    public string  resModel;//要加载的模型资源
    public override int Key
    {
        get {  return ID; }
    }
  public override void Decode(byte[] byteArr, ref int bytePos)
   {
        ReadInt32(ref byteArr,ref bytePos,out ID);
        ReadString(ref byteArr,ref bytePos,out name);
        ReadString(ref byteArr,ref bytePos,out resModel);
  }
}
/// <summary>
///表文件AI角色配置.xlsx管理类
/// </summary>
public class roleInfoTableManager : TableManager<roleInfo>
{
   public static readonly roleInfoTableManager instance=new roleInfoTableManager();
}