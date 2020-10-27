using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AlphaLogo : MonoBehaviour
{
    public GameObject StartPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake() {
        transform.Find("Image").GetComponent<Image>().DOFade(0, 2f).OnComplete(()=> {
            StartPanel.SetActive(true);
            int value = 1;
            DOTween.To(() => value, x => value = x, 5, 0.5f).SetUpdate(true).OnComplete(()=> { this.gameObject.SetActive(false); });
            
            
        });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
