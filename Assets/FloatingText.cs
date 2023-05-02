using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Transform go;
    public Vector3 endValue;
    public int jumpPower = 10;
    public float animTime = 1;
    public float moveUp = 1;
    public bool autoDispose;
    public void init(string text,Vector3 pos,Color color,float stayTime, bool autoDispose = true)
    {
        gameObject.SetActive(true);
        GetComponentInChildren<Text>().text = text;
        GetComponentInChildren<Text>().color = color;
        transform.position = pos;
        if (autoDispose) Destroy(gameObject, 1);
        animTime = stayTime;
        this.autoDispose = autoDispose;
    }
    // Start is called before the first frame update
    void Start()
    {
        go.DOPunchScale(endValue,animTime, jumpPower);
        go.DOLocalMoveY(moveUp, animTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string text, bool isFinalUpdate = true)
    {
        go.localScale *= 1.6f;
        go.DOPunchScale(endValue, .2f, jumpPower);
        go.DOPunchRotation(new Vector3(0, 0, -50), .2f, 10, .4f);
        GetComponentInChildren<Text>().text = text;

        if (isFinalUpdate)
        {
            FlingAndDestroyText();
        }
    }

    public void FlingAndDestroyText(float prependWaitTime = 1.5f)
    {

        Sequence sequence = DOTween.Sequence();
        sequence.PrependInterval(prependWaitTime);
        sequence.Append(go.DOLocalJump(new Vector3(3, 2, 0), 1, 1, 1));
        sequence.Join(go.DOScale(0, .8f));

        sequence.Play();
            
        Destroy(gameObject, 3);
    }
}