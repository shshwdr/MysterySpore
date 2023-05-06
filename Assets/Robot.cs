using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private CircleCollider2D collider;

    public bool isActivated = false;
    public float activateTime = 0;
    private int scientistInView = 0;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Human")
        {
            scientistInView++;
            isActivated = true;
            activateTime = Time.time;
            animator.SetTrigger("activate");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Human")
        {
            
            scientistInView--;
            if (scientistInView <= 0)
            {
            
                isActivated = false;
                animator.SetTrigger("deactivate");
            }
        }
    }

}
