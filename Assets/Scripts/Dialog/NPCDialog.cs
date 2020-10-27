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
        public Paragraph(bool boolean,string text) {
            isPlayerA = boolean;
            this.text = text;
        }
    }
    [SerializeField]
    public string playerAName;
    public string playerBName;
    public Sprite playerAImage;
    public Sprite playerBImage;
    [SerializeField]
    public List<Paragraph> paragraphList;
    [SerializeField]
    public List<Paragraph> ASideParagraph;
    [SerializeField]
    public List<Paragraph> BSideParagraph;
    public string usualText;
    public string tipText;
    public bool haveBranch;
    public bool playAfterChosen;
    public string optionAText;
    public string optionBText;

    private bool isInRange;
    public bool isChosen;
    public bool isFinish;
    public int textIndex;
    public bool isPlayParagraph;
    public bool haveTip;
    public bool isNPC2;
    public bool isFinal;
    public bool isBoom;
    public bool needExchange;

    Tweener textShow;

    public GameObject blackPanel;
    public GameObject dialogPanel;
    public Text dialogName;
    public Text dialogText;
    public Image PanelPlayerA;
    public Image PanelPlayerB;
    public Button OptionA;
    public Button OptionB;
    public GameObject Tip;
    public GameObject EndPanel;
    public GameObject FKeyText;
    public AudioClip FinalClip;
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
        if (Input.GetKeyDown(KeyCode.F))
            if (isInRange) DialogStart();
        if (Input.GetMouseButtonDown(0)) {
            if (isFinish) return;
            if (isPlayParagraph) ParagraphStop();
            else ParagraphStart();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            FKeyText.SetActive(true);
            isInRange = true;
            Debug.Log("PlayerIn!");
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            FKeyText.SetActive(false);
            isInRange = false;
            Debug.Log("PlayerOut!");
        }
    }
    public void DialogStart() {//开始播放
        Time.timeScale = 0f;
        if (isFinal) {
            GameObject.Find("MainCamera").transform.Find("BGM").GetComponent<AudioSource>().clip = FinalClip;
            GameObject.Find("MainCamera").transform.Find("BGM").GetComponent<AudioSource>().Play();
            GameObject.Find("MainCamera").transform.Find("BGM").GetComponent<AudioSource>().volume = 0.9f;
        }
        PanelPlayerA.sprite = playerAImage;
        PanelPlayerB.sprite = playerBImage;
        dialogPanel.SetActive(true);
        isFinish = false;
        textIndex = 0;
        ParagraphStart();
    }
    void ParagraphStart() {//开始某一句对话

        if (textIndex == paragraphList.Count) {
            if (!haveBranch||isChosen) {//如果没有分支或已选择过
                paragraphList.Clear();
                paragraphList.Add(new Paragraph(false,usualText));
                 PlayFinish();
            }
            else {
                
                OptionA.gameObject.SetActive(true);
                OptionB.gameObject.SetActive(true);
                OptionA.GetComponent<Button>().onClick.RemoveAllListeners();
                OptionB.GetComponent<Button>().onClick.RemoveAllListeners();

                OptionA.GetComponent<Button>().onClick.AddListener(delegate { this.Choose(true); });
                OptionB.GetComponent<Button>().onClick.AddListener(delegate { this.Choose(false); });

                OptionA.transform.Find("Text").GetComponent<Text>().text = optionAText;
                OptionB.transform.Find("Text").GetComponent<Text>().text = optionBText;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
        }
        if (isFinal&&isChosen&& textIndex == 3) {
            playerAName = "\"你\"";
            playerBName = "\"伊恩\"";
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
            textShow = dialogText.DOText(paragraphList[textIndex].text, paragraphList[textIndex].text.Length * 0.1f)
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
        int value = 1;
        DOTween.To(() => value, x => value = x, 5, 0f).SetUpdate(true).OnComplete(() => {
            Time.timeScale = 1;
            if (isBoom) {
                GameObject.Find("Canvas").transform.Find("BlackPanel").gameObject.GetComponent<Image>().DOFade(0, 0f).OnComplete(
                    ()=> { GameObject.Find("Canvas").transform.Find("BlackPanel").gameObject.SetActive(false); });
            }
            if (needExchange) {
                GameObject.Find("RecordManager").GetComponent<RecordManager>().canBackPlay = true;
            }
            isFinish = true;
            dialogPanel.SetActive(false);
            if (haveTip) {
                Tip.SetActive(true);
                Tip.GetComponent<TipController>().PlayText(tipText);
                if (isNPC2) haveTip = false;
            }
            if (isFinal) {
                EndPanel.SetActive(true);
                GameObject.Find("MainCamera").transform.Find("BGM").GetComponent<AudioSource>().Stop();
                DOTween.To(() => value, x => value = x, 5, 0.5f).SetUpdate(true).OnComplete(()=> {
                    GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.Find("WhitePanel").gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.Find("BackgroundImage").gameObject.SetActive(false);
                });

            }
        });
    }
    public void Choose(bool ASide) {
        if (ASide) {
            paragraphList = ASideParagraph;
        }
        else paragraphList = BSideParagraph;
        if (isNPC2) {
            if (ASide) {
                GameObject.Find("BothButton1").transform.Find("Cube").GetComponent<BothButton>().isASide = true;
                GameObject.Find("BothButton2").transform.Find("Cube").GetComponent<BothButton>().isASide = true;
            }
            else {
                GameObject.Find("BothButton1").transform.Find("Cube").GetComponent<BothButton>().isASide = false;
                GameObject.Find("BothButton2").transform.Find("Cube").GetComponent<BothButton>().isASide = false;
            }
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OptionA.gameObject.SetActive(false);
        OptionB.gameObject.SetActive(false);
        isChosen = true;
        textIndex = 0;
        if (isFinal) {
            GameObject.Find("FinalTrigger").GetComponent<FinishLevel>().ChangeCG();
            dialogPanel.GetComponent<Image>().DOFade(0, 0.1f).SetUpdate(true);
            PanelPlayerA.gameObject.SetActive(false);
            PanelPlayerB.gameObject.SetActive(false);
        }
        if (playAfterChosen) {
            ParagraphStart();
        }

    }
}
