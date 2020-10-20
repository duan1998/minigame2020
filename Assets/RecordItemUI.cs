using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RecordStatus
{
    NoRecord,
    Record,
    Recording,
    PlayingNormal,
    PlayingBack
}

public class RecordItemUI : MonoBehaviour
{
    GameObject recordingLabelObj;
    GameObject playingNormalLabelObj;
    GameObject playingBackLabelObj;
    GameObject noRecordLabelObj;
    GameObject recordLabelObj;

    Animator animator;

    private void Awake()
    {
        recordingLabelObj=transform.Find("RecordingLabel").gameObject;
        playingNormalLabelObj = transform.Find("PlayingNormalLabel").gameObject;
        playingBackLabelObj = transform.Find("PlayingBackLabel").gameObject;
        noRecordLabelObj = transform.Find("NoRecordLabel").gameObject;
        recordLabelObj= transform.Find("RecordLabel").gameObject;
        animator = GetComponent<Animator>();
    }

    public void Refresh(RecordStatus status)
    {
        switch(status)
        {
            case RecordStatus.NoRecord:
                recordingLabelObj.SetActive(false);
                playingNormalLabelObj.SetActive(false);
                playingBackLabelObj.SetActive(false);
                recordLabelObj.SetActive(false);
                noRecordLabelObj.SetActive(true);
                break;
            case RecordStatus.Recording:
                recordingLabelObj.SetActive(true);
                playingNormalLabelObj.SetActive(false);
                playingBackLabelObj.SetActive(false);
                recordLabelObj.SetActive(false);
                noRecordLabelObj.SetActive(false);
                break;
            case RecordStatus.PlayingNormal:
                recordingLabelObj.SetActive(false);
                playingNormalLabelObj.SetActive(true);
                playingBackLabelObj.SetActive(false);
                recordLabelObj.SetActive(false);
                noRecordLabelObj.SetActive(false);
                break;
            case RecordStatus.PlayingBack:
                recordingLabelObj.SetActive(false);
                playingNormalLabelObj.SetActive(false);
                playingBackLabelObj.SetActive(true);
                recordLabelObj.SetActive(false);
                noRecordLabelObj.SetActive(false);
                break;
            case RecordStatus.Record:
                recordingLabelObj.SetActive(false);
                playingNormalLabelObj.SetActive(false);
                playingBackLabelObj.SetActive(false);
                recordLabelObj.SetActive(true);
                noRecordLabelObj.SetActive(false);
                break;

        }
    }

    public void PlayWrongAnimatoin()
    {
        animator.Play("Wrong", 0, 0);
    }
    public void PlayEnlargeAnimation()
    {
        animator.Play("Enlarge", 0, 0);
    }
    public void PlayShrinkAnimation()
    {
        animator.Play("Shrink", 0, 0);
    }
    public void Init(RecordStatus status)
    {
        Refresh(status);
    }
    public void PlayNoticeAnimation()
    {
        if (playingBackLabelObj.activeSelf||playingNormalLabelObj.activeSelf)
        {
            animator.Play("NoticePlaying", 0, 0);
        }

    }
}
