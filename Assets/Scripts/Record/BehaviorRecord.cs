using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BehaviorRecord
{
    //帧list  每一条帧序列中可能先后包括不确定数目的行为
    [SerializeField]
    private List<Behaviour> frameRecordList;


    public BehaviorRecord()
    {
        frameRecordList = new List<Behaviour>();
    }

    public void AddBehaviour(Behaviour behaviour)
    {
        frameRecordList.Add(behaviour);
    }
    public int FrameCount
    {
        get
        {
            if (frameRecordList!=null)
                return frameRecordList.Count;
            return 0;
        }
    }
    public Behaviour this[int index]
    {
        get 
        {
            if (frameRecordList != null)
                return frameRecordList[index];
            return null;
        }
    }
}
