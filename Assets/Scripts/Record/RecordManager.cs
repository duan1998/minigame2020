using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    // 录像idx从0开始到2


    public struct RecordModeItem
    {
        public int recordIndex;
        public bool bBack;
        public RecordModeItem(int index,bool bBack)
        {
            recordIndex = index;
            this.bBack = bBack;
        }
    }

    [Range(1,3)]
    public int maxSaveRecordCount;

    public Transform playerCharacterTrans;
    public ThirdPersonUserControl playerCtrl;
    public GhostCharacter ghost;

    private RecordManager() { }
    private static RecordManager instance;
    public static RecordManager Instance
    {
        get=>instance;
    }

    [SerializeField]
    private BehaviorRecord[] records;
    [SerializeField]
    private RecordStatus[] status;

    [HideInInspector]
    public BehaviorRecord curRecordingRecord;
    [HideInInspector]
    public bool bRecording;

    private int curSelectRecordIndex;  //当前选中的

    private List<RecordModeItem> willPlayRecordBuffer; //将要播放的record的序列缓冲，不重复
    private int curPlayingBufferIndex; // 正在播放缓冲里的record的index

    [SerializeField]
    private bool canBackPlay;

    [SerializeField] MainUI mainUI;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        curRecordingRecord = null;
        curSelectRecordIndex = 0;
        bRecording = false;
        records = new BehaviorRecord[3];
        willPlayRecordBuffer = new List<RecordModeItem>();
        canBackPlay = false;
        // 默认状态
        status = new RecordStatus[maxSaveRecordCount];
        mainUI.Set(status);
    }

    public bool StartRecording()
    {
        if (bRecording || bPlaying)
        {
            mainUI.Wrong(curSelectRecordIndex);
            return false;
        }
        mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.Recording);
        curRecordingRecord = new BehaviorRecord();
        bRecording = true;
        return true;
    }
    public void StopRecording()
    {
        records[curSelectRecordIndex] = curRecordingRecord;
        mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.Record);
        bRecording = false;
        curRecordingRecord = null;
    }

   
    public bool bPlaying = false;
    public void PlayRecord()
    {
        if (bRecording || records[curSelectRecordIndex]==null || records[curSelectRecordIndex].FrameCount==0)
        {
            mainUI.Wrong(curSelectRecordIndex);
            return;
        }
        if (willPlayRecordBuffer.Count <= 0)
        {
            ghost.transform.position = playerCharacterTrans.position;
            ghost.transform.rotation = playerCharacterTrans.rotation;
            ghost.gameObject.SetActive(true);
            willPlayRecordBuffer.Clear();
            willPlayRecordBuffer.Add(new RecordModeItem(curSelectRecordIndex,false));
            // 播放缓冲里的第一个
            curPlayingBufferIndex = 0;
            mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.PlayingNormal);

        }
        else
        {
            for(int i=0;i<willPlayRecordBuffer.Count;i++)
            {
                if (willPlayRecordBuffer[i].recordIndex== curSelectRecordIndex)
                {
                    // 已经在播放缓冲里了，不能再播放
                    mainUI.NoticePlayingOrHasWouldPlay(curSelectRecordIndex);
                    return;
                }
            }
            willPlayRecordBuffer.Add(new RecordModeItem(curSelectRecordIndex, false));
            mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.PlayingNormal);
        }
        bPlaying = true;
        ghost.SetBehaviourRecord(records[willPlayRecordBuffer[0].recordIndex], PlayOver, willPlayRecordBuffer[0].bBack);
    }

    public void PlayBackRecord()
    {
        if (bRecording || records[curSelectRecordIndex] == null || records[curSelectRecordIndex].FrameCount == 0)
        {
            mainUI.Wrong(curSelectRecordIndex);
            return;
        }
        if (willPlayRecordBuffer.Count<=0)
        {
            ghost.transform.position = playerCharacterTrans.position;
            ghost.transform.rotation = playerCharacterTrans.rotation;
            ghost.gameObject.SetActive(true);
            willPlayRecordBuffer.Clear();
            willPlayRecordBuffer.Add(new RecordModeItem(curSelectRecordIndex, true));
            // 播放缓冲里的第一个
            curPlayingBufferIndex = 0;
            mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.PlayingBack);
        }
        else
        {
            for (int i = 0; i < willPlayRecordBuffer.Count; i++)
            {
                if (willPlayRecordBuffer[i].recordIndex == curSelectRecordIndex)
                {
                    // 已经在播放缓冲里了，不能再播放
                    mainUI.NoticePlayingOrHasWouldPlay(curSelectRecordIndex);
                    return;
                }
            }
            willPlayRecordBuffer.Add(new RecordModeItem(curSelectRecordIndex, true));
            mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.PlayingBack);
        }
        ghost.SetBehaviourRecord(records[willPlayRecordBuffer[0].recordIndex], PlayOver, willPlayRecordBuffer[0].bBack);

    }


    void PlayOver()
    {
        mainUI.RefreshRecordStatus(willPlayRecordBuffer[curPlayingBufferIndex].recordIndex, RecordStatus.Record);
        curPlayingBufferIndex++;
        if(curPlayingBufferIndex>=willPlayRecordBuffer.Count)
        {
            bPlaying = false;
            ghost.gameObject.SetActive(false);
            willPlayRecordBuffer.Clear();
        }
        else
        {

            ghost.SetBehaviourRecord(records[willPlayRecordBuffer[curPlayingBufferIndex].recordIndex], PlayOver, willPlayRecordBuffer[curPlayingBufferIndex].bBack);
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            // 正放
            PlayRecord();
        }
        else if(canBackPlay&&Input.GetKeyDown(KeyCode.T))
        {
            // 倒放
            PlayBackRecord();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            // 向左选中
            SelectPrev();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            // 向右选中
            SelectNext();
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            // 录制
            //StartRecording();
        }
    }

    // 0 1 2
    private void SelectNext()
    {
        if (curSelectRecordIndex+1>=maxSaveRecordCount)
        {
            return;
        }
        if (bRecording)
        {
            mainUI.Wrong(curSelectRecordIndex);
            return;
        }
        curSelectRecordIndex++;
        // 通知UI更新
        mainUI.FocusNext(curSelectRecordIndex);
    }
    private void SelectPrev()
    {
        if (curSelectRecordIndex-1 < 0 )
        {
            return;
        }
        if (bRecording)
        {
            mainUI.Wrong(curSelectRecordIndex);
            return;
        }
        curSelectRecordIndex--;
        // 通知UI更新
        mainUI.FocusPrev(curSelectRecordIndex);

    }


    public void RecordCountLevelUp()
    {
        if (maxSaveRecordCount == 3)
            return;
        maxSaveRecordCount = 3;
        // 默认状态
        RecordStatus firstStatus = status[0];
        status = new RecordStatus[maxSaveRecordCount];
        status[0] = firstStatus;
        mainUI.Set(status);
    }
    public void CanCarryLevelUp()
    {
        playerCtrl.CanCarry = true;
    }
    public void CanBackPlayLevelUp()
    {
        canBackPlay = true;
    }



}
