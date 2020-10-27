using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LadderButton : MonoBehaviour
{
    public GameObject ladder;
    public float m_moveTime = 2f;
    public float m_rotationZ = 0f;
    public bool IsTemporary = false;
    public bool isTouch = false;
    public int numsOfCharactor;
    private float m_initialRotationZ;
    public AudioClip ButtonClip;
    public AudioClip GearClip;
    // Start is called before the first frame update
    private void Awake() {
        numsOfCharactor = 0;
        m_initialRotationZ = ladder.transform.rotation.z;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow"||other.gameObject.tag=="Box") {
            numsOfCharactor++;
            transform.parent.DOScaleY(0.5f, 0.1f);
            if (!IsTemporary&&ladder.GetComponent<BridgeStatues>() != null && !ladder.GetComponent<BridgeStatues>().isLock)
                ladder.GetComponent<BridgeStatues>().isLock = true;
            ladder.transform.DORotate(new Vector3(ladder.transform.rotation.x, ladder.transform.rotation.y, m_rotationZ), m_moveTime);
            GameObject.Find("MainCamera").transform.Find("Effect1").GetComponent<AudioSource>().clip = ButtonClip;
            GameObject.Find("MainCamera").transform.Find("Effect1").GetComponent<AudioSource>().Play();
            if (!IsTemporary && !isTouch) {
                GameObject.Find("MainCamera").transform.Find("Effect2").GetComponent<AudioSource>().clip = GearClip;
                GameObject.Find("MainCamera").transform.Find("Effect2").GetComponent<AudioSource>().Play();
                isTouch = true;
            }
            if (IsTemporary) {
                if (ladder.GetComponent<BridgeStatues>().isLock == false) {
                    GameObject.Find("MainCamera").transform.Find("Effect2").GetComponent<AudioSource>().clip = GearClip;
                    GameObject.Find("MainCamera").transform.Find("Effect2").GetComponent<AudioSource>().Play();
                }

            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shadow" || other.gameObject.tag == "Box") {
            numsOfCharactor--;
            if (numsOfCharactor == 0&& IsTemporary) {
                transform.parent.DOScaleY(1f, 0.1f);
                if (ladder.GetComponent<BridgeStatues>()!=null&&!ladder.GetComponent<BridgeStatues>().isLock)
                ladder.transform.DORotate(new Vector3(ladder.transform.rotation.x, ladder.transform.rotation.y, m_initialRotationZ), m_moveTime);
            }
        }
    }
}
