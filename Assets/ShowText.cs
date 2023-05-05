using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    public TMP_Text label;
    public GameObject panel;
    public string textKey;
    public float showTime = 1;
    public void Show()
    {
        if (DialogueManager.Instance.DialogueDict.ContainsKey(textKey))
        {
            panel.SetActive(true);
            label.text = DialogueManager.Instance.DialogueDict[textKey][0][0].text;
            StartCoroutine(hide());
        }
    }

    IEnumerator hide()
    {
        yield return new WaitForSeconds(showTime);
        
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
            Show();
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
