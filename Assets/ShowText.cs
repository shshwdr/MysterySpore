using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShowText : MonoBehaviour
{
    public TMP_Text label;
    public GameObject panel;
    public string textKey;
    public void Show(string extra, float time)
    {
        if (DialogueManager.Instance.DialogueDict.ContainsKey(textKey + extra))
        {
            var potentialWords = DialogueManager.Instance.DialogueDict[textKey + extra];
            panel.SetActive(true);
            label.text = potentialWords[Random.Range(0,potentialWords.Count)][0].text;
            StartCoroutine(hide(time));
        }
    }

    IEnumerator hide(float time)
    {
        yield return new WaitForSeconds(time);
        
        panel.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (GetComponent<HumanAI>())
        {
            return;
        }
        if (col.collider.tag == "Vine")
        {
            StopAllCoroutines();
            Show("",DialogueManager.Instance.popupShowTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
