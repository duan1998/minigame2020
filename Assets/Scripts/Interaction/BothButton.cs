using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BothButton : MonoBehaviour
{
    public int numsOfCharactor;
    public bool IsPress;
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
                Debug.Log("Success!");
                AbilityLock.GetComponent<AbilityGet>().isLock = false;
                AbilityLock.GetComponent<MeshRenderer>().material.DOColor(Color.red, 10f);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow" || other.gameObject.tag == "Box") {
            numsOfCharactor--;
            if (numsOfCharactor == 0) {
                transform.parent.DOScaleY(1f, 0.1f);
                GameObject.Find("ThirdPersonController").GetComponent<ThirdPersonUserControl>().CanTrainMove = false;
            }
        }
    }
}
