using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Human : MonoBehaviour
{
    private CircleCollider2D collider;

    private HPBar hpbar;
    public float maxHP = 15;
    private float currentHP = 10;
    private float hpDecreaseSpeed = 2;
    private int collideByVineCount = 0;
    private int numberOfPoints = 12;
    private float checkRadius;
    private string vineTag = "Vine";
    private LayerMask layerMask;
    private bool isRunningAway = false;
    private float runAwayTime = 0.3f;
    private float strugglingAwayTime = 1f;

    public bool isSuffering => collideByVineCount > 0;

    private HumanAI humanAi;
    private void Awake()
    {
        hpbar = GetComponentInChildren<HPBar>();
        collider = GetComponent<CircleCollider2D>();
        currentHP = maxHP;
        hpbar.init(maxHP);
        checkRadius = collider.radius * collider.transform.lossyScale.x;
        layerMask = 1 << LayerMask.NameToLayer(vineTag);
        humanAi = GetComponent<HumanAI>();
        ;
    }


    // Update is called once per frame
    void Update()
    {
        // if (!isRunningAway)
        // {
        //         
        //     RunAway();
        // }
        if (collideByVineCount > 0)
        {
            
            currentHP -= hpDecreaseSpeed * Time.deltaTime;
            if (currentHP <= 0)
            {
                Destroy(gameObject);
                MPProgressManager.Instance.recoverEnergy(MPProgressManager.Instance.recoverFromHuman);
            }
            hpbar.updateCurrent(currentHP);
            if (!humanAi.isEscaping)
            {
                humanAi.Escape();
            }
        }
    }

    
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == vineTag)
        {
            collideByVineCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == vineTag)
        {
            collideByVineCount--;
        }
    }
}
