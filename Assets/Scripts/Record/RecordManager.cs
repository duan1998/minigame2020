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
        public RecordModeItem(int index, bool bBack)
        {
            recordIndex = index;
            this.bBack = bBack;
        }
    }

    [Range(1, 3)]
    public int maxSaveRecordCount;

    public Transform playerCharacterTrans;
    public ThirdPersonUserControl playerCtrl;
    public GhostCharacter ghost;

    private RecordManager() { }
    private static RecordManager instance;
    public static RecordManager Instance
    {
        get => instance;
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
    public bool canBackPlay;

    [SerializeField] MainUI mainUI;

    [Tooltip("录制的时长")]
    public float recordDuration;//录制的时长
    private float curRecordRemainingTime;

    private void Awake()
    {
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        mainUI.Set(maxSaveRecordCount);
    }

    public bool StartRecording()
    {
        if (bRecording || bPlaying)
        {
            mainUI.Wrong(curSelectRecordIndex);
            return false;
        }
        curRecordRemainingTime = recordDuration;
        mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.Recording,0);
        curRecordingRecord = new BehaviorRecord();
        bRecording = true;
        return true;
    }
    public void StopRecording()
    {
        records[curSelectRecordIndex] = curRecordingRecord;
        mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.Record,1);
        bRecording = false;
        curRecordingRecord = null;
    }


    public bool bPlaying = false;
    public void PlayRecord()
    {
        if (bRecording || records[curSelectRecordIndex] == null || records[curSelectRecordIndex].FrameCount == 0)
        {
            mainUI.Wrong(curSelectRecordIndex);
            return;
        }
        if (willPlayRecordBuffer.Count <= 0)
        {
            ghost.transform.position = playerCharacterTrans.position;

            ghost.transform.forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            ghost.gameObject.SetActive(true);
            willPlayRecordBuffer.Clear();
            willPlayRecordBuffer.Add(new RecordModeItem(curSelectRecordIndex, false));
            // 播放缓冲里的第一个
            curPlayingBufferIndex = 0;
            mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.PlayingNormal,0);
            bPlaying = true;
            ghost.SetBehaviourRecord(willPlayRecordBuffer[curPlayingBufferIndex].recordIndex,records[willPlayRecordBuffer[0].recordIndex], PlayOver, willPlayRecordBuffer[0].bBack, OnPlayingEvent);
        }
        else
        {
            for (int i = 0; i < willPlayRecordBuffer.Count; i++)
            {
                if (willPlayRecordBuffer[i].recordIndex == curSelectRecordIndex)
                {
                    // 已经在播放缓冲里了，不能再播放
                    mainUI.Wrong(curSelectRecordIndex);
                    return;
                }
            }
            willPlayRecordBuffer.Add(new RecordModeItem(curSelectRecordIndex, false));
            mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.PlayingNormal,0);
        }

    }

    public void PlayBackRecord()
    {
        if (bRecording || records[curSelectRecordIndex] == null || records[curSelectRecordIndex].FrameCount == 0)
        {
            mainUI.Wrong(curSelectRecordIndex);
            return;
        }
        if (willPlayRecordBuffer.Count <= 0)
        {
            ghost.transform.position = playerCharacterTrans.position;
            ghost.transform.forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            ghost.gameObject.SetActive(true);
            willPlayRecordBuffer.Clear();
            willPlayRecordBuffer.Add(new RecordModeItem(curSelectRecordIndex, true));
            // 播放缓冲里的第一个
            curPlayingBufferIndex = 0;
            mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.PlayingBack,0);
            bPlaying = true;
            ghost.SetBehaviourRecord(willPlayRecordBuffer[curPlayingBufferIndex].recordIndex,records[willPlayRecordBuffer[0].recordIndex], PlayOver, willPlayRecordBuffer[0].bBack, OnPlayingEvent);
        }
        else
        {
            for (int i = 0; i < willPlayRecordBuffer.Count; i++)
            {
                if (willPlayRecordBuffer[i].recordIndex == curSelectRecordIndex)
                {
                    // 已经在播放缓冲里了，不能再播放
                    mainUI.Wrong(curSelectRecordIndex);
                    return;
                }
            }
            willPlayRecordBuffer.Add(new RecordModeItem(curSelectRecordIndex, true));
            mainUI.RefreshRecordStatus(curSelectRecordIndex, RecordStatus.PlayingBack,0);
        }


    }


    public void PlayOver()
    {
        mainUI.RefreshRecordStatus(willPlayRecordBuffer[curPlayingBufferIndex].recordIndex, RecordStatus.Record,1);
        curPlayingBufferIndex++;
        if (curPlayingBufferIndex >= willPlayRecordBuffer.Count)
        {
            bPlaying = false;
            ghost.PutDownBox();
            ghost.gameObject.SetActive(false);

            willPlayRecordBuffer.Clear();
        }
        else
        {

            ghost.SetBehaviourRecord(willPlayRecordBuffer[curPlayingBufferIndex].recordIndex, records[willPlayRecordBuffer[curPlayingBufferIndex].recordIndex], PlayOver, willPlayRecordBuffer[curPlayingBufferIndex].bBack, OnPlayingEvent);
        }
    }

    void OnPlayingEvent(int recordIndex,bool bBack,float progress)
    {
        if (bBack)
        {
            mainUI.RefreshRecordStatus(recordIndex, RecordStatus.PlayingBack, progress);
        }
        else
        {
            mainUI.RefreshRecordStatus(recordIndex, RecordStatus.PlayingNormal, progress);
        }
    }




    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ghost.tag = "Shadow";
            // 正放
            PlayRecord();
        }
        else if (canBackPlay && Input.GetKeyDown(KeyCode.R))
        {
            ghost.tag = "Shadow";
            // 倒放
            PlayBackRecord();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 向左选中
            SelectPrev();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // 向右选中
            SelectNext();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ghost.tag = "Untagged";
            // 预览
            PreviewGhostPosition(false);
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            ghost.tag = "Untagged";
            // 预览
            PreviewGhostPosition(true);
        }
        // 录制
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartRecording();
        }



        if(bRecording)
        {
            curRecordRemainingTime -= Time.deltaTime;
            mainUI.RefreshRecordStatus(curSelectRecordIndex,RecordStatus.Recording, (recordDuration - curRecordRemainingTime) / recordDuration);
            if (curRecordRemainingTime<=0)
            {
                // 完成
                StopRecording();
            }
        }

    }
    private void PreviewGhostPosition(bool bBack)
    {
        if (records[curSelectRecordIndex]==null|| records[curSelectRecordIndex].FrameCount <= 0) return;
        ghost.transform.position = playerCharacterTrans.position;

        ghost.transform.forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        ghost.gameObject.SetActive(true);
        for (int i = 0; i < records[curSelectRecordIndex].FrameCount; i++)
        {
            ghost.transform.rotation = Quaternion.Euler(ghost.transform.eulerAngles.x, ghost.transform.eulerAngles.y + records[curSelectRecordIndex][i].deltaYAxis, ghost.transform.eulerAngles.z);

            Vector3 deltaOffset = ghost.transform.forward * records[curSelectRecordIndex][i].deltaHorizontalDisplacement;
            deltaOffset.y = records[curSelectRecordIndex][i].deltaVerticalDisplacement;
            if (bBack)
            {
                ghost.transform.position -= deltaOffset;
            }
            else
            {
                ghost.transform.position += deltaOffset;
            }
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
        // 清理
        records = new BehaviorRecord[3];
        // 默认状态
        mainUI.Set(maxSaveRecordCount);
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
