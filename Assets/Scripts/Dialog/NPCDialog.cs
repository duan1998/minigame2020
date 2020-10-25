using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class NPCDialog : MonoBehaviour
{
    [System.Serializable]
    public struct Paragraph {
        public bool isPlayerA;
        public string text;
    }
    [SerializeField]
    public string playerAName;
    public string playerBName;
    public Image playerAImage;
    public Image playerBImage;
    [SerializeField]
    public List<Paragraph> paragraphList;
    [SerializeField]
    public List<Paragraph> ASideParagraph;
    [SerializeField]
    public List<Paragraph> BSideParagraph;
    public string usualText;
    public bool haveBranch;
    public bool playAfterChosen;
    public string optionAText;
    public string optionBText;

    private bool isInRange;
    public bool isChosen;
    public bool isFinish;
    private int textIndex;
    public bool isPlayParagraph;

    Tweener textShow;

    public GameObject dialogPanel;
    public Text dialogName;
    public Text dialogText;
    public Image PanelPlayerA;
    public Image PanelPlayerB;
    public Button OptionA;
    public Button OptionB;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake() {
        isInRange = false;
        textIndex = 0;
        isFinish = true;
        isPlayParagraph = false;
        isChosen = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            if (isInRange) DialogStart();
        if (Input.GetMouseButtonDown(0)) {
            if (isFinish) return;
            if (isPlayParagraph) ParagraphStop();
            else ParagraphStart();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            isInRange = true;
            Debug.Log("PlayerIn!");
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            isInRange = false;
            Debug.Log("PlayerOut!");
        }
    }
    void DialogStart() {//开始播放
        Time.timeScale = 0f;
        dialogPanel.SetActive(true);
        isFinish = false;
        textIndex = 0;
        ParagraphStart();
    }
    void ParagraphStart() {//开始某一句对话

        if (textIndex == paragraphList.Count) {
            if (!haveBranch||isChosen) {//如果没有分支或已选择过
                PlayFinish();
            }
            else {
                OptionA.gameObject.SetActive(true);
                OptionB.gameObject.SetActive(true);
                OptionA.transform.Find("Text").GetComponent<Text>().text = optionAText;
                OptionB.transform.Find("Text").GetComponent<Text>().text = optionBText;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
        }
        isPlayParagraph = true;
        dialogText.text = "";
        if (textIndex < paragraphList.Count) {
            if (paragraphList[textIndex].isPlayerA) {
                PanelPlayerA.DOFade(1, 0.1f).SetUpdate(true);
                PanelPlayerB.DOFade(0.5f, 0.1f).SetUpdate(true);
                dialogName.text = playerAName;
            }
            else {
                PanelPlayerA.DOFade(0.5f, 0.1f).SetUpdate(true);
                PanelPlayerB.DOFade(1f, 0.1f).SetUpdate(true);
                dialogName.text = playerBName;
            }
            textShow = dialogText.DOText(paragraphList[textIndex].text, paragraphList[textIndex].text.Length * 0.2f)
                .OnComplete(ParagraphOver).SetUpdate(true).SetEase(Ease.Linear);
        }
    }
    void ParagraphStop() {//停止动画，直接显示文本
        textShow.Kill();
        dialogText.DOText(paragraphList[textIndex].text, 0.1f).OnComplete(ParagraphOver).SetUpdate(true).SetEase(Ease.Linear);
    }
    void ParagraphOver() {
        textIndex++;
        isPlayParagraph = false;     
    }
    void PlayUsualText() {

    }
    void PlayFinish() {
        Time.timeScale = 1;
        isFinish = true;
        dialogPanel.SetActive(false);
    }
    public void ChooseASide() {

    }
    public void Choose(bool ASide) {
        if (ASide) {
            paragraphList = ASideParagraph;
        }
        else paragraphList = BSideParagraph;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OptionA.gameObject.SetActive(false);
        OptionB.gameObject.SetActive(false);
        isChosen = true;
        textIndex = 0;
        if (playAfterChosen) {
            ParagraphStart();
        }

    }
}
