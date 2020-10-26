using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class BothButton : MonoBehaviour
{
    public GameObject DialogAB;
    public GameObject DialogBoom;
    public GameObject blackPanel;
    public int numsOfCharactor;
    public bool IsPress;
    public bool isASide;
    public bool AlreadyPress;
    public GameObject otherButton;
    public GameObject AbilityLock;
    // Start is called before the first frame update
    void Start() {
        numsOfCharactor = 0;
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow" || other.gameObject.tag == "Box") {
            numsOfCharactor++;
            transform.parent.DOScaleY(0.5f, 0.1f);
            IsPress = true;
            if (otherButton.GetComponent<BothButton>().IsPress) {
                if (AlreadyPress) return;
                AlreadyPress = true;
                Debug.Log("Success!");
                blackPanel.SetActive(true);
                blackPanel.GetComponent<Image>().DOFade(1, 0.5f).SetUpdate(true).OnComplete(()=> {
                    DialogBoom.GetComponent<NPCDialog>().DialogStart();
                    NPCDialog npc = GameObject.Find("npc2").GetComponent<NPCDialog>();
                    npc.isFinish = false;
                    npc.haveBranch = false;
                    npc.isNPC2 = false;
                    npc.textIndex = 0;
                    npc.usualText = DialogAB.GetComponent<NPCDialog>().usualText;
                    npc.haveTip = true;
                    npc.tipText = DialogAB.GetComponent<NPCDialog>().tipText;
                    npc.needExchange = true;
                    if (isASide) {
                        npc.paragraphList = DialogAB.GetComponent<NPCDialog>().ASideParagraph;
                    }
                    else {
                        npc.paragraphList = DialogAB.GetComponent<NPCDialog>().BSideParagraph;
                    }
                });
                
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow" || other.gameObject.tag == "Box") {
            numsOfCharactor--;
            if (numsOfCharactor == 0) {
                transform.parent.DOScaleY(1f, 0.1f);
                IsPress = false;
            }
        }
    }
}
