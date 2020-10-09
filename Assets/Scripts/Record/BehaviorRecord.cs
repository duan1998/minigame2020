using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BehaviorRecord : ISerializationCallbackReceiver
{
    //唯一标识
    public int id;
    //本条记录的名字
    public string name;
    //帧list  每一条帧序列中可能先后包括不确定数目的行为
    public List<List<BaseBehaviour>> frameRecordList;

    public BehaviorRecord(int id,string name)
    {
        this.id = id;
        this.name = name;
    }

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        
    }
}
