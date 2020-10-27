using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTips : MonoBehaviour
{
    public bool isBox;
    public GameObject TextPanel;
    public string tipText;
    public bool isTouch;
    // Start is called before the first frame update
    void Start()
    {
        isTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" || other.tag == "Shadow" || other.tag == "Box") {
            if (isBox)
                if (other.tag != "Box") return;
            if (isTouch) return;
            TextPanel.SetActive(true);
            TextPanel.GetComponent<TipController>().PlayText(tipText);
            isTouch = true;
        }
    }
}
