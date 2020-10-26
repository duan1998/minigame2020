using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    public Image recordRemainingTimeBar;

    public float recordDuration;//录制的时长
    private float curRecordRemainingTime;

    public ThirdPersonUserControl playerCtrl;
    public CinemachineFreeLook followCamera;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        if (RecordManager.Instance.bRecording)
        {
            //记录行为
            curRecordRemainingTime -= Time.deltaTime;
            if (curRecordRemainingTime <= 0)
            {
                HideRecordRemainingTimeBar();
                RecordManager.Instance.StopRecording();
            }
            else
            {
                recordRemainingTimeBar.fillAmount = curRecordRemainingTime / recordDuration;

            }
        }

        if (Input.GetMouseButtonDown(0)&&!RecordManager.Instance.bRecording)
        {
            if (GameObject.Find("DialogPanel") != null) return;
            StartRecord();
        }
        if(Input.GetKeyDown(KeyCode.F1)&&!RecordManager.Instance.bPlaying)
        {
            //RecordManager.Instance.PlayRecord();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)&&!RecordManager.Instance.bPlaying)
        {
            //RecordManager.Instance.PlayBackRecord(0);
        }
    }
    void StartRecord()
    {
        curRecordRemainingTime = recordDuration;


        bool result=RecordManager.Instance.StartRecording();
        if(result)
            ShowRecordRemainingTimeBar();

    }

    void ShowRecordRemainingTimeBar()
    {
        recordRemainingTimeBar.transform.parent.gameObject.SetActive(true);
    }
    void HideRecordRemainingTimeBar()
    {
        recordRemainingTimeBar.transform.parent.gameObject.SetActive(false);
    }

    
}
