using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGet : MonoBehaviour
{
    public int AbilityType = 1;
    public GameObject panel;
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
            if (AbilityType == 0) {
                panel.SetActive(false);
            }
            else
                if (AbilityType == 1) {
                GameObject.Find("ThirdPersonController").GetComponent<ThirdPersonUserControl>().CanCarry = true;
            }
            else
                if (AbilityType == 2) {
                GameObject.Find("RecordManager").GetComponent<RecordManager>().canBackPlay = true;
            }
            else 
                if (AbilityType == 3) {
                GameObject.Find("ThirdPersonController").GetComponent<ThirdPersonUserControl>().CanExChange = true;
            }
            Destroy(gameObject);
        }
    }
}
