using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class MoviePlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<VideoPlayer>().loopPointReached += StopPlay;
    }

    public void StopPlay(VideoPlayer video) {
        this.GetComponent<Image>().DOFade(1, 1.5f).OnComplete(() => { SceneManager.LoadScene(1); });
    }
    public void StopPlay() {
        this.GetComponent<Image>().DOFade(1, 1.5f).OnComplete(() => { SceneManager.LoadScene(1); });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
