using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AbilityGet : MonoBehaviour
{
    public int AbilityType = 1;
    public GameObject Dialog;
    public GameObject WhitePanel;
    public bool isLock = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (isLock) return;
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow") {
            WhitePanel.SetActive(true);
            WhitePanel.GetComponent<Image>().DOFade(1, 1f).OnComplete(()=> {
                gameObject.SetActive(false);
                WhitePanel.GetComponent<Image>().DOFade(0, 1f).OnComplete(Ability);
                }
            );

        }
    }
    public void Ability() {
        Destroy(gameObject);
        WhitePanel.SetActive(false);
        if (AbilityType == 0) {
            GameObject.Find("RecordManager").GetComponent<RecordManager>().RecordCountLevelUp();
            Dialog.GetComponent<NPCDialog>().DialogStart();
        }
        else
    if (AbilityType == 1) {
            GameObject.Find("ThirdPersonController").GetComponent<ThirdPersonUserControl>().CanCarry = true;
            Dialog.GetComponent<NPCDialog>().DialogStart();
        }
        else
    if (AbilityType == 2) {
            GameObject.Find("RecordManager").GetComponent<RecordManager>().canBackPlay = true;
        }
        else
    if (AbilityType == 3) {
            GameObject.Find("ThirdPersonController").GetComponent<ThirdPersonUserControl>().CanExChange = true;
            Dialog.GetComponent<NPCDialog>().DialogStart();
        }

    }
}
