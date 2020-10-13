using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    //record的固定path前缀为StreamingAssets的
    public string PersistentDataPath;
    private const string FileNamePrefix = "Record_";

    private int currentRecordIdValue;
    public int maxSaveRecordCount;

    public ThirdPersonUserControl playerCtrl;
    public Transform ghostTrans;

    private RecordManager() { }
    private static RecordManager instance;
    public static RecordManager Instance
    {
        get=>instance;
    }


    public List<BehaviorRecord> recordList=new List<BehaviorRecord>();

    private void Awake()
    {
        instance = this;
        PersistentDataPath = Application.dataPath + "Record";
        if(!PlayerPrefs.HasKey("RecordId"))
        {
            PlayerPrefs.SetInt("RecordId", 1);
            currentRecordIdValue = 1;
        }
        else
        {
            currentRecordIdValue= PlayerPrefs.GetInt("RecordId");
        }
    }


    public void RemoveRecord(int index)
    {
        if (recordList.Count <= index)
            return;
        recordList.RemoveAt(index);
    }

    public bool SaveRecord(BehaviorRecord record)
    {
        if (maxSaveRecordCount <= 0) return false;
        if (recordList.Count<maxSaveRecordCount)
        {
            recordList.Add(record);
        }
        else
        {
            recordList[maxSaveRecordCount - 1] = record;
        }
        return true;
    }
    bool isPlaying = false;
    int recordIndex = 0;
    int currentPlayingRecord;
    public void PlayRecord(int index)
    {
        if (recordList.Count <= index) return;
        if (isPlaying) return;
        isPlaying = true;
        recordIndex = 0;
        currentPlayingRecord = index;
        ghostTrans.position = playerCtrl.transform.position;
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            if (recordIndex < recordList[currentPlayingRecord].frameRecordList.Count)
            {
                ghostTrans.position += recordList[currentPlayingRecord].frameRecordList[recordIndex].deltaDisplacement;
                recordIndex++;
            }
            else
            {
                isPlaying = false;
            }
        }
        
    }


    public int GetNewId()
    {
        ++currentRecordIdValue;
        PlayerPrefs.SetInt("RecordId", currentRecordIdValue);
        return currentRecordIdValue;
    }

    public bool IsFull()
    {
        if(recordList.Count>=maxSaveRecordCount)
        {
            return true;
        }
        return false;
    }



}
