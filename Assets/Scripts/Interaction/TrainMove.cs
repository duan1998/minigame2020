using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TrainMove : MonoBehaviour
{
    public GameObject TheOtherTrainMove;
    public GameObject blackPanel;
    public bool CanMove = true;
    // Start is called before the first frame update
    void Start()
    {
        CanMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player"&&CanMove&&other.GetComponent<ThirdPersonUserControl>().CanTrainMove) {
            Vector3 nextPosition = new Vector3(TheOtherTrainMove.transform.position.x, TheOtherTrainMove.transform.position.y + 1, TheOtherTrainMove.transform.position.z);
            transform.parent.DOScaleY(0.5f, 0.1f);
            blackPanel.SetActive(true);
            blackPanel.GetComponent<Image>().DOFade(1, 1.5f).OnComplete(()=> {
                TheOtherTrainMove.GetComponent<TrainMove>().CanMove=false;
                float value=0f;
                DOTween.To(() => value, x => value = x, 5, 1f).OnComplete(() => { TheOtherTrainMove.GetComponent<TrainMove>().CanMove = true; });
                GameObject.Find("ThirdPersonController").transform.DOMove(nextPosition, 0f);
                blackPanel.GetComponent<Image>().DOFade(0, 1.5f).OnComplete(() => {
                    blackPanel.SetActive(false);
                    transform.parent.DOScaleY(0.5f, 0.1f);
                });
            });

        };
    }
    private void OnTriggerExit(Collider other) {
        transform.parent.DOScaleY(1f, 0.1f);
    }
}
