using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TipController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayText(string text) {
        this.GetComponent<Text>().text = text;
        this.GetComponent<Text>().DOFade(1, 0.1f).OnComplete(() => {
            this.GetComponent<Text>().DOFade(0, 4f).OnComplete(()=> { this.gameObject.SetActive(false); });
        });
    }
}
