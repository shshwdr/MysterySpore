using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KoganeUnityLib;
using Sinbad;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueInfo
{
    public string text;
    public string type;
    public string speaker;
    public int hasNext;
    public string special;

    public string Text => text;
}

public class DialogueManager : Singleton<DialogueManager>
{
    public  Dictionary<string, List<List<DialogueInfo>>> DialogueDict =
        new Dictionary<string, List<List<DialogueInfo>>>();


    public bool shouldShowDialogues = true;


    public TMP_Text dialogueLabel;
    public TMP_Text speakerLabel;
    public GameObject dialoguePanel;
    public GameObject selectionPanel;
    public Image bk;
    public Button dialogueButton;

    public Sprite generalDialogueBK;

    public Color lightTextColor;
    public Color darkTextColor;

    private TMP_Typewriter typeWriter;
    private float m_speed = 15;

    private bool dialogueClick = false;

    private bool isInAnim = false;

    private void Start()
    {
        if (DialogueDict.ContainsKey(GameManager.Instance.level.ToString()))
        {
            StartCoroutine( showDialogueGeneral(GameManager.Instance.level.ToString()));
        }
    }

    public IEnumerator showDialogueGeneral(string key)
    {
        var DialogueLists = DialogueDict[key][0];

        yield return StartCoroutine(showDialogue(DialogueLists, DialogueState.posAnswer));
    }

    public void showDragDialogue()
    {
        if (!GameManager.Instance.hasDragged&& GameManager.Instance.level == 0)
        {
            GameManager.Instance.hasDragged = true;
            StartCoroutine( showDialogueGeneral("AfterDrag"));
        }
    }

    public void showKillDialogue()
    {
        if (!GameManager.Instance.hasKilled && GameManager.Instance.level == 0)
        {
            GameManager.Instance.hasKilled = true;
            StartCoroutine( showDialogueGeneral("FirstKill"));
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        typeWriter = GetComponent<TMP_Typewriter>();
        var dialogInfos = CsvUtil.LoadObjects<DialogueInfo>("dialogues");
        for (int i = 0; i < dialogInfos.Count; i++)
        {
            var info = dialogInfos[i];
            if (info.text == "")
            {
                continue;
            }

            if (!DialogueDict.ContainsKey(info.type))
            {
                DialogueDict[info.type] = new List<List<DialogueInfo>>();
            }

            List<DialogueInfo> dialogList = new List<DialogueInfo>();
            dialogList.Add(info);
            while (info.hasNext == 1)
            {
                i++;

                info = dialogInfos[i];
                if (info.text == "")
                {
                    break;
                }
                dialogList.Add(info);
            }

            DialogueDict[info.type].Add(dialogList);
        }

        dialoguePanel.SetActive(false);

        dialogueButton.onClick.AddListener(() =>
        {
            if (isInAnim)
            {
                
            }
            else if (typeWriter.IsFinished)
            {
                dialogueClick = true;
            }
            else
            {
                typeWriter.Skip();
            }
        });
    }


    public IEnumerator showWinDialogue()
    {
        var DialogueLists = DialogueDict["win"][0];

        yield return StartCoroutine(showDialogue(DialogueLists, DialogueState.posAnswer));
    }

    public IEnumerator showLoseDialogue(bool isKilling)
    {
        var DialogueLists = DialogueDict["lose"][isKilling ? 1 : 0];

        yield return StartCoroutine(showDialogue(DialogueLists, DialogueState.posAnswer));
    }

    public IEnumerator showTutorialDialogue(int id)
    {
        var DialogueLists = DialogueDict["tutorial"][id];

        yield return StartCoroutine(showDialogue(DialogueLists, DialogueState.posAnswer));
    }

    public IEnumerator showRegularDialogue(int score)
    {
        string selectDictKey = null;
        switch (score)
        {
            case 0:
                selectDictKey = "0";
                break;
            case 1:
                selectDictKey = "1";
                break;
            case 2:
            case 3:
                selectDictKey = "2";
                break;
            case 4:
            case 5:
                selectDictKey = "4";
                break;
            case -1:
                selectDictKey = "-1";
                break;
            case -2:
            case -3:
                selectDictKey = "-2";
                break;
            case -4:
            case -5:
                selectDictKey = "-4";
                break;
        }

        if (score >= 6)
        {
            selectDictKey = "6";
        }
        else if (score <= -6)
        {
            selectDictKey = "-6";
        }

        var DialogueLists = DialogueDict[selectDictKey];

        var pickedDialogueList = DialogueLists[Random.Range(0, DialogueLists.Count)];


        DialogueState state = DialogueState.negAnswer;
        if (score < 0)
        {
            state = DialogueState.posAnswer;
        }
        else if (score > 0)
        {
            state = DialogueState.negAnswer;
        }
        else
        {
            state = DialogueState.neutralAnswer;
        }

        yield return StartCoroutine(showDialogue(pickedDialogueList, state));
    }

    enum DialogueState
    {
        negAnswer,
        posAnswer,
        neutralAnswer
    }

    IEnumerator InternEnter()
    {
        isInAnim = true;
        float moveTime = 2;
        var intern = GameObject.Find("TheFirstIntern").transform;
        intern.DOMove(intern.Find("TargetPosition").transform.position,moveTime);
        intern.GetComponentInChildren<Animator>().SetTrigger("idle");
        yield return new WaitForSeconds(moveTime);
        isInAnim = false;
    }
    IEnumerator showDialogue(List<DialogueInfo> dialogueList, DialogueState state)
    {
        dialoguePanel.SetActive(true);
        // foreach (var b in selectionPanel.GetComponentsInChildren<Button>())
        // {
        //     b.gameObject.SetActive(false);
        // }

        //selectionPanel.SetActive(false);

        //
        foreach (var info in dialogueList)
        {
            if (info.speaker != "")
            {
                switch (info.special)
                {
                    case "internEnter":
                        yield return StartCoroutine(InternEnter());
                        break;
                }
            }


            speakerLabel.text = info.speaker;
            dialogueLabel.text = ""; //info.text;
            var showText = info.Text;
            typeWriter.Play
            (
                text: showText,
                speed: m_speed,
                onComplete: () =>
                {
                    //Debug.Log("完了");
                });
            dialogueLabel.GetComponent<VertexShakeA>().enabled = state == DialogueState.negAnswer;
            dialogueLabel.color = lightTextColor;

            yield return new WaitUntil(() => dialogueClick);
            dialogueClick = false;
        }


        dialoguePanel.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
    }
}