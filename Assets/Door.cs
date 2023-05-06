using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        animator =GetComponent<Animator>();
        collider =GetComponent<Collider2D>();
    }

    private bool isOpened = false;
    public void Open()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.door, transform.position);
        animator.enabled = true;
        collider.enabled = true;
        isOpened = true;
        //animator.Play()
    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Open();
        }

        if (isOpened)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (collider.OverlapPoint(mousePosition))
                {
                    GameManager.Instance.GotoNextLevel();
                }
            }
        }
    }
}
