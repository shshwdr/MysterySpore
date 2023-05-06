using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCinematic : MonoBehaviour
{
    public bool isLose = false;
    public Sprite[] images;

    public Image image;
    private bool dialogueClick = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLose)
        {
            
            if (GameManager.Instance.skipCinematic())
            {
                gameObject.SetActive(false);
                return;
            }
            
            image.gameObject.SetActive(true);
            GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                dialogueClick = true;
            });
            
            StartCoroutine(startone());
        }
    }

    public IEnumerator startone()
    {
        
        yield return StartCoroutine(showImage());
            
        StartCoroutine( DialogueManager.Instance. showDialogueGeneral(GameManager.Instance.level.ToString()));
    }

    public IEnumerator showLose()
    {
        image.gameObject.SetActive(true);
        GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            dialogueClick = true;
        });
        yield return StartCoroutine(showImage());
        
    }
    
    IEnumerator showImage()
    {
        foreach (var sprite in images)
        {
            image.sprite = sprite;
            
            yield return new WaitUntil(() => dialogueClick);
            dialogueClick = false;
        }
        image.gameObject.SetActive(false);
        //GameManager.Instance.skipCinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
