using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    //record的固定path前缀为StreamingAssets的
    public string PersistentDataPath;
    private const string FileNamePrefix = "Record_";



    private RecordManager() { }
    private static RecordManager instance;
    public static RecordManager Instance
    {
        get=>instance;
    }


    private List<BehaviorRecord> recordList=new List<BehaviorRecord>();
    private Dictionary<int, BehaviorRecord> recordDic = new Dictionary<int, BehaviorRecord>();

    private void Awake()
    {
        instance = this;
        PersistentDataPath = Application.dataPath + "Record";
    }


    private bool RemoveRecord(int id)
    {
        if (!recordDic.ContainsKey(id)) 
            return false;

        recordList.Remove(recordDic[id]);
        recordDic.Remove(id);
        return true;
    }
    
    private bool AddRecord(int id)
    {
        if (recordDic.ContainsKey(id))
            OverrideRecord(id);

    }


    //覆盖掉原有的数据
    private void OverrideRecord(int id)
    {
        //TODO 在固定的文件夹中
    }

}
