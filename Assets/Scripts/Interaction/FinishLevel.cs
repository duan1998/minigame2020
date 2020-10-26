using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FinishLevel : MonoBehaviour
{
    public GameObject Dialog;
    public GameObject BackgroundImage;
    public GameObject WhitePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow") {
            WhitePanel.SetActive(true);
            WhitePanel.GetComponent<Image>().DOFade(1, 2f).OnComplete(() => {
                PlayFinialText();
            }
            );

        }
    }
    public void PlayFinialText() {
        Dialog.GetComponent<NPCDialog>().DialogStart();
    }
    public void ChangeCG() {
        BackgroundImage.SetActive(true);
        BackgroundImage.GetComponent<Image>().DOFade(1, 1f).SetUpdate(true);
    }
}
