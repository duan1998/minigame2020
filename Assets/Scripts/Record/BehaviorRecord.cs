using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BehaviorRecord : ISerializationCallbackReceiver
{
    //唯一标识
    public int id;
    //本条记录的名字
    public string name;
    //帧list  每一条帧序列中可能先后包括不确定数目的行为
    [NonSerialized]
    public List<Behaviour> frameRecordList;
    public Behaviour[] frameRecords;

    public BehaviorRecord(int id,string name)
    {
        this.id = id;
        this.name = name;
    }

    public void OnAfterDeserialize()
    {
        frameRecordList = new List<Behaviour>(frameRecords);
    }

    public void OnBeforeSerialize()
    {
        frameRecords = frameRecordList.ToArray();
    }

    public void AddBehaviour(Behaviour behaviour)
    {
        frameRecordList.Add(behaviour);
    }
    void Clear()
    {
        frameRecordList.Clear();
    }
}
