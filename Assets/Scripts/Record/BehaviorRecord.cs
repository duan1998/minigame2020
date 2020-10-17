using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BehaviorRecord
{
    //本条记录的名字
    public string name;
    //帧list  每一条帧序列中可能先后包括不确定数目的行为
    [SerializeField]
    private List<Behaviour> frameRecordList=new List<Behaviour>();

    public BehaviorRecord(string name)
    {
        this.name = name;
    }

    public void AddBehaviour(Behaviour behaviour)
    {
        frameRecordList.Add(behaviour);
    }
    public int FrameCount
    {
        get{ return frameRecordList.Count; }
    }
    public Behaviour this[int index]
    {
        get { return frameRecordList[index]; }
    }
}
