using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Portal : MonoBehaviour
{
    public float SearchRadius=1.15f;
    public bool PlayerOnCube;
    public List<GameObject> PortalList;
    public GameObject lightA;
    public GameObject lightB;
    // Start is called before the first frame update
    void Start()
    {
        PlayerOnCube = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerOnCube) {
            if (SearchPlayer(transform,SearchRadius)==null) {
                //transform.parent.DOScaleY(1f, 0.1f);
                PlayerOnCube = false;
                lightA.SetActive(true);
                lightB.SetActive(false);
                return;
            }
            TryToExChange();
        }
            
    }
    public GameObject SearchPlayer(Transform tran,float radius) {
        Collider[] colliders = Physics.OverlapSphere(tran.position, SearchRadius);

        if (colliders.Length <= 0) return null;

        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].gameObject.tag == "Player" || colliders[i].gameObject.tag == "Shadow") return colliders[i].gameObject;
        return null;
    }
    public void TryToExChange() {
        for (int i = 0; i < PortalList.Count; i++) {
            if (PortalList[i].GetComponent<Portal>().PlayerOnCube) {
                //交换实际上就是把玩家传送到影子位置
                GameObject playerA = SearchPlayer(transform,SearchRadius+0.5f);
                GameObject playerB = SearchPlayer(PortalList[i].transform,SearchRadius+0.5f);
                GameObject ShadowPortal = this.gameObject;
                if (playerA.tag == "Shadow") {
                    GameObject temp = playerA;
                    playerA = playerB;
                    playerB = temp;
                    ShadowPortal = PortalList[i];
                }
                if (playerA != null && playerB != null) {
                    float value = 1;
                    DOTween.To(() => value, x => value = x, 5, 0.5f).OnComplete(() => {
                        ShadowPortal.GetComponent<Portal>().PlayerOnCube = false;
                        //ShadowPortal.transform.parent.DOScaleY(1f, 0.1f);
                        ShadowPortal.GetComponent<Portal>().lightA.SetActive(true);
                        ShadowPortal.GetComponent<Portal>().lightB.SetActive(false);
                        Transform temp = playerA.transform;
                        playerA.transform.DOMove(playerB.transform.position, 0.1f);
                        //playerB.transform.DOMove(temp.position,0.1f);
                        GameObject.Find("RecordManager").GetComponent<RecordManager>().PlayOver();
                        return;
                    });
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" || other.tag == "Shadow") {
            //if (!GameObject.Find("ThirdPersonController").GetComponent<ThirdPersonUserControl>().CanExChange) return;
            PlayerOnCube = true;
            lightA.SetActive(false);
            lightB.SetActive(true);
            
            //transform.parent.DOScaleY(0.5f, 0.1f);
        }
    }
}
