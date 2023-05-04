using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCinematic : MonoBehaviour
{
    public Sprite[] images;

    public Image image;
    private bool dialogueClick = false;
    // Start is called before the first frame update
    void Start()
    {
        // if (GameManager.Instance.skipCinematic)
        // {
        //     gameObject.SetActive(false);
        //     return;
        // }
        image.gameObject.SetActive(true);
        StartCoroutine(showImage());
        
        GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            dialogueClick = true;
        });
    }
    
    IEnumerator showImage()
    {
        foreach (var sprite in images)
        {
            image.sprite = sprite;
            
            yield return new WaitUntil(() => dialogueClick);
            dialogueClick = false;
        }
        gameObject.SetActive(false);
        //GameManager.Instance.skipCinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}