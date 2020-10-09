using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Behaviour : ISerializationCallbackReceiver
{
    public Vector3 deltaDisplacement;//位移增量
    [NonSerialized]
    public BehaviourType type;
    public string[] types;
    [NonSerialized]
    private List<string> typeList;

    public Behaviour(Vector3 deltaDisplacement, BehaviourType type)
    {
        this.deltaDisplacement = deltaDisplacement;
        this.type = type;
    }
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (types == null) return;
        type = 0;
        for (int i=0;i<types.Length;i++)
        {
            type |= (BehaviourType)Enum.Parse(typeof(BehaviourType), types[i]);
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        typeList = new List<string>();
        foreach (BehaviourType item in Enum.GetValues(typeof(BehaviourType)))
        {
            typeList.Add(item.ToString());
        }
        types = typeList.ToArray();
    }
}

