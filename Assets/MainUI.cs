using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public struct Pair<T,T1>
    {
        public T first;
        public T1 second;
        public Pair(T first, T1 second)
        {
            this.first = first;
            this.second = second;
        }
    }

    public RecordItemUI[] recordItems;

    public void Set(int num)
    {
        for (int i = 0; i < num; i++)
        {
            recordItems[i].gameObject.SetActive(true);
            recordItems[i].Init();
        }
        for(int i=num;i<recordItems.Length;i++)
        {
            recordItems[i].gameObject.SetActive(false);
        }
        recordItems[0].PlayEnlargeAnimation();
    }
    
    public void Wrong(int idx)
    {
        if (idx >= recordItems.Length)
            return;
        recordItems[idx].PlayWrongAnimatoin();
    }

    public void FocusNext(int idx)
    {
        print("willCur" + idx);
        if (idx >= recordItems.Length || idx < 0) return;
        if (idx - 1 < 0)
            return;
        recordItems[idx-1].PlayShrinkAnimation();
        recordItems[idx].PlayEnlargeAnimation();
    }
    public void FocusPrev(int idx)
    {
        print("willCur" + idx);
        if (idx >= recordItems.Length || idx < 0) return;
        if (idx + 1 >= recordItems.Length)
            return;
        recordItems[idx+1].PlayShrinkAnimation();
        recordItems[idx].PlayEnlargeAnimation();
    }

    //// 提醒正在播放或者待播放
    //public void NoticePlayingOrHasWouldPlay(int idx)
    //{
    //    if (idx >= recordItems.Length || idx < 0) return;
    //    recordItems[idx].PlayNoticeAnimation();
    //}
    public void RefreshRecordStatus(int idx,RecordStatus status,float progress)
    {
        if (idx >= recordItems.Length || idx < 0) return;
        recordItems[idx].Refresh(status,progress);
    }
    
}
