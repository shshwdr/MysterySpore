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

    [HideInInspector]
    public Animator animator;
    public bool isSuffering => collideByVineCount > 0;

    private HumanAI humanAi;

    public bool isDead = false;
    private void Awake()
    {
        hpbar = GetComponentInChildren<HPBar>();
        collider = GetComponent<CircleCollider2D>();
        currentHP = maxHP;
        if (hpbar)
        {
            hpbar.init(maxHP);
        }
        checkRadius = collider.radius * collider.transform.lossyScale.x;
        layerMask = 1 << LayerMask.NameToLayer(vineTag);
        humanAi = GetComponent<HumanAI>();
        animator = GetComponentInChildren<Animator>();
        ;
    }

    private void Start()
    {
        HumanManager.Instance.AddHuman(GetComponent<HumanAI>());
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
            if (HumanManager.Instance.isLose)
            {
                return;
            }
            currentHP -= hpDecreaseSpeed * Time.deltaTime;
            if (currentHP <= 0)
            {
                Die();
            }
            hpbar.updateCurrent(currentHP);
            // if (!humanAi.isEscaping)
            // {
            //     humanAi.Escape();
            // }
        }
    }

    void Die()
    {
        if (!isDead)
        {
            DialogueManager.Instance.showKillDialogue();
            isDead = true;
            if (GetComponent<Human>().animator)
            {
                GetComponent<Human>().animator.SetTrigger("die");
            }

            if(Random.Range(0,100)>50)
            {
                
                if (GetComponent<ShowText>())
                {
                    GetComponent<ShowText>().Show(" Dying");
                }
            }
            RemoveAllComponentsExceptTransform();
            //Destroy(gameObject);
            MPProgressManager.Instance.recoverEnergy(MPProgressManager.Instance.recoverFromHuman);
            HumanManager.Instance.RemoveHuman(GetComponent<HumanAI>());
            Destroy(hpbar.gameObject);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.humanDying, transform.position);
        }
    }

    void RemoveAllComponentsExceptTransform()
    {
        Component[] components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
            // Skip the Transform component
            if (!(component is Transform) && !(component is SpriteRenderer)&& !(component is Animator) && !(component is ShowText))
            {
                Destroy(component);
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
