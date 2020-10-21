using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChargeTrain : MonoBehaviour
{
    public float m_moveTime = 2f;
    public float m_rotationZ = 0f;
    public bool IsTemporary = false;
    public int numsOfCharactor;
    private float m_initialRotationZ;
    // Start is called before the first frame update
    void Start()
    {
        numsOfCharactor = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow" || other.gameObject.tag == "Box") {
            numsOfCharactor++;
            transform.parent.DOScaleY(0.5f, 0.1f);
            GameObject.Find("ThirdPersonController").GetComponent<ThirdPersonUserControl>().CanTrainMove = true;
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
