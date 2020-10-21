using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ExchangeDoor : MonoBehaviour
{
    public int numsOfCharactor;
    public bool IsPress;
    public GameObject otherDoor;
    // Start is called before the first frame update
    void Start()
    {
        numsOfCharactor = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow" || other.gameObject.tag == "Box") {
            numsOfCharactor++;
            transform.parent.DOScaleY(0.5f, 0.1f);
            IsPress = true;
            if (otherDoor.GetComponent<BothButton>().IsPress) {
                Debug.Log("Success!");
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
