using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    public Image recordRemainingTimeBar;
    public GameObject recordNameInputObj;
    public InputField recordNameInputField;
    public Button recordNameSubmitBtn;

    public float recordDuration;//录制的时长
    private float curRecordRemainingTime;

    public ThirdPersonUserControl playerCtrl;
    public CinemachineFreeLook followCamera;
    private void Awake()
    {
        recordNameSubmitBtn.onClick.AddListener(OnSubmitBtnClick);
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
            curRecordRemainingTime -= Time.fixedDeltaTime;
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

        if (Input.GetKeyDown(KeyCode.P)&&!RecordManager.Instance.bRecording)
        {
            StartRecord();
        }
        if(Input.GetKeyDown(KeyCode.F1)&&!RecordManager.Instance.bPlaying)
        {
            RecordManager.Instance.PlayRecord(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)&&!RecordManager.Instance.bPlaying)
        {
            RecordManager.Instance.PlayBackRecord(0);
        }
    }
    void StartRecord()
    {
        curRecordRemainingTime = recordDuration;
        recordNameInputObj.SetActive(true);
        recordNameInputField.text = "";
        Cursor.lockState = CursorLockMode.None;
        followCamera.enabled = false;
        Cursor.visible = true;
        playerCtrl.enabled = false;
        
    }

    void ShowRecordRemainingTimeBar()
    {
        recordRemainingTimeBar.transform.parent.gameObject.SetActive(true);
    }
    void HideRecordRemainingTimeBar()
    {
        recordRemainingTimeBar.transform.parent.gameObject.SetActive(false);
    }

    void OnSubmitBtnClick()
    {
        if (recordNameInputField.text == null || recordNameInputField.text == "")
        {
            Debug.Log("无法保存空名字的录像");
        }
        else
        {
            RecordManager.Instance.StartRecording(recordNameInputField.text);
            recordNameInputObj.SetActive(false);
            ShowRecordRemainingTimeBar();

            playerCtrl.enabled = true;
            followCamera.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
}
