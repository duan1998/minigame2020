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
    public Animator recorderAnimator;

    public Image recordImageBar;
    public Image backPlayBar;
    public Image normalPlayBar;

    private void Start()
    {
        recordImageBar.fillAmount = 0;
        backPlayBar.fillAmount = 0;
        normalPlayBar.fillAmount = 0;
    }

    public void Refresh(RecordStatus status,float fillAmount)
    {
        switch (status)
        {
            case RecordStatus.NoRecord:
                recordImageBar.fillAmount = 0;
                backPlayBar.fillAmount = 0;
                normalPlayBar.fillAmount = 0;
                break;
            case RecordStatus.Recording:
                recordImageBar.fillAmount = fillAmount;
                backPlayBar.fillAmount = 0;
                normalPlayBar.fillAmount = 0;
                break;
            case RecordStatus.PlayingNormal:
                recordImageBar.fillAmount = 1;
                backPlayBar.fillAmount = 0;
                normalPlayBar.fillAmount = fillAmount;

                break;
            case RecordStatus.PlayingBack:
                recordImageBar.fillAmount = 1;
                backPlayBar.fillAmount = fillAmount;
                normalPlayBar.fillAmount = 0;
                break;
            case RecordStatus.Record:
                recordImageBar.fillAmount = 1;
                backPlayBar.fillAmount = 0;
                normalPlayBar.fillAmount = 0;
                break;

        }
    }

 
    public void PlayWrongAnimatoin()
    {
        recorderAnimator.Play("Wrong", 0, 0);
    }
    public void PlayEnlargeAnimation()
    {
        recorderAnimator.Play("Enlarge", 0, 0);
    }
    public void PlayShrinkAnimation()
    {
        recorderAnimator.Play("Shrink", 0, 0);
    }
    public void Init()
    {
        Refresh(RecordStatus.NoRecord, 0);
    }
    //public void PlayNoticeAnimation()
    //{
    //    if (playingBackLabelObj.activeSelf||playingNormalLabelObj.activeSelf)
    //    {
    //        animator.Play("NoticePlaying", 0, 0);
    //    }

    //}
}
