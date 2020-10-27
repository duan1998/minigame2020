using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class BeginPanelController : MonoBehaviour
{
    public Text AnyKeyText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake() {
        Time.timeScale = 0;
        transform.Find("Panel").GetComponent<Image>().DOFade(0, 1.5f).SetUpdate(true) ;
        AnyKeyText.DOFade(0f, 2f).OnComplete(AllAlphaText).SetUpdate(true);
    }
    public void HaveAlphaText() {
        AnyKeyText.DOFade(0f, 2f).OnComplete(AllAlphaText).SetUpdate(true);
    }
    public void AllAlphaText() {
        AnyKeyText.DOFade(1f, 1f).OnComplete(HaveAlphaText).SetUpdate(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) this.GetComponent<Image>().DOFade(0, 1.5f)
                .SetUpdate(true).OnComplete(() => { Time.timeScale = 1; this.gameObject.SetActive(false); });
            
    }
}
