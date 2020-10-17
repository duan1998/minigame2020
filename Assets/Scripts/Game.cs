using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    public bool bRecording;
    public Image recordRemainingTimeBar;
    public GameObject recordNameInputObj;
    public InputField recordNameInputField;
    public Button recordNameSubmitBtn;

    public float recordDuration;//录制的时长
    public float curRecordRemainingTime;
    public ThirdPersonUserControl playerCtrl;
    public ThirdPersonCharacter characterCtrl;
    public Transform recordItemRoot;

    private string[] keyBoards = {"F1","F2","F3"};
    private void Awake()
    {
        recordNameSubmitBtn.onClick.AddListener(OnSubmitBtnClick);
    }
    // Start is called before the first frame update
    void Start()
    {
        bRecording = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)&&!bRecording)
        {
            StartRecord();
        }
        if (Input.GetKeyDown(KeyCode.Escape)&& bRecording)
        {
            //终止录制并不做保存
            OverRecord();

        }
        if(Input.GetKey(KeyCode.D)&&Input.GetKeyDown(KeyCode.F1))
        {
            //删除F1
            RecordManager.Instance.RemoveRecord(0);

        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.F2))
        {
            //删除F2
            RecordManager.Instance.RemoveRecord(1);
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.F3))
        {
            //删除F3
            RecordManager.Instance.RemoveRecord(2);
        }


        if (Input.GetKeyDown(KeyCode.F1))
        {
            //第一条
            RecordManager.Instance.PlayRecord(0);
        }
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            //第二条
            RecordManager.Instance.PlayRecord(1);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            //第三条
            RecordManager.Instance.PlayRecord(2);
        }
        

    }
    void StartRecord()
    {
        curRecordRemainingTime = recordDuration;
        recordNameInputObj.SetActive(true);
        recordNameInputField.text = "";
    }
    void OverRecord()
    {
        bRecording = false;
        HideRecordRemainingTimeBar();
        bool bSuccess=RecordManager.Instance.SaveRecord(tempBehaviourRecord);
        if(bSuccess)
        {
            if (RecordManager.Instance.IsFull())
            {
                int idx = recordItemRoot.childCount - 1;
                //替换Text内容
                recordItemRoot.GetChild(idx).GetComponentInChildren<Text>().text = keyBoards[idx] + "   " + tempBehaviourRecord.name;
            }
            else
            {
                //生成一个新的
                GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/RecordItem"), recordItemRoot);
                obj.GetComponentInChildren<Text>().text = keyBoards[recordItemRoot.childCount - 1] + "   " + tempBehaviourRecord.name;
            }
        }
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
            tempBehaviourRecord = new BehaviorRecord(RecordManager.Instance.GetNewId(), recordNameInputField.text);
            recordNameInputObj.SetActive(false);
            ShowRecordRemainingTimeBar();
            bRecording = true;
        }
    }


    private BehaviorRecord tempBehaviourRecord;
    private void FixedUpdate()
    {
        if (bRecording)
        {
            //记录行为
            tempBehaviourRecord.AddBehaviour(new Behaviour(playerCtrl.deltaDisplacement, characterCtrl.behaviourType));
            curRecordRemainingTime -= Time.fixedDeltaTime;
            if (curRecordRemainingTime <= 0)
            {
                OverRecord();

            }
            else
            {
                recordRemainingTimeBar.fillAmount = curRecordRemainingTime / recordDuration;

            }
        }
    }


}
