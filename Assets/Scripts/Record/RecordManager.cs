using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{

    public int maxSaveRecordCount;

    public ThirdPersonUserControl playerCtrl;
    public GhostCharacter ghost;

    private RecordManager() { }
    private static RecordManager instance;
    public static RecordManager Instance
    {
        get=>instance;
    }

    [SerializeField]
    private List<BehaviorRecord> recordList=new List<BehaviorRecord>();

    [HideInInspector]
    public BehaviorRecord curRecordingRecord;
    [HideInInspector]
    public bool bRecording;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        curRecordingRecord = null;
        bRecording = false;
    }

    public void StartRecording(string name)
    {
        curRecordingRecord = new BehaviorRecord(name);
        bRecording = true;
    }
    public void StopRecording()
    {
         if (recordList.Count <= 0)
            recordList.Add(curRecordingRecord);
        else
            recordList[0] = curRecordingRecord;
        bRecording = false;
    }


    public void RemoveRecord(int index)
    {
        if (recordList.Count <= index)
            return;
        recordList.RemoveAt(index);
    }

   
    public bool bPlaying = false;
    public void PlayRecord(int index)
    {
        if (recordList.Count <= index) return;
        if (bPlaying) return;
        ghost.gameObject.SetActive(true);
        ghost.SetBehaviourRecord(recordList[index],PlayOver,false);
    }

    public void PlayBackRecord(int index)
    {
        if (recordList.Count <= index) return;
        if (bPlaying) return;
        ghost.gameObject.SetActive(true);
        ghost.SetBehaviourRecord(recordList[index], PlayOver, true);
    }


    void PlayOver()
    {
        bPlaying = false;
    }

}
