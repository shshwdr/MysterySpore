using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    private Collider2D collider;

    private HPBar hpbar;
    private float maxHP = 10;
    private float currentHP = 10;
    private float hpDecreaseSpeed = 5;
    private int collideByVineCount = 0;
    private void Awake()
    {
        hpbar = GetComponentInChildren<HPBar>();
        collider = GetComponent<Collider2D>();
        currentHP = maxHP;
        hpbar.init(maxHP);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collideByVineCount > 0)
        {
            
            currentHP -= hpDecreaseSpeed * Time.deltaTime;
            hpbar.updateCurrent(currentHP);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Vine")
        {
            collideByVineCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Vine")
        {
            collideByVineCount--;
        }
    }
}
