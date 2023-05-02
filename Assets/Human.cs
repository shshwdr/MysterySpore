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

    // Start is called before the first frame update
    void Start()
    {
        
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
            }
            hpbar.updateCurrent(currentHP);
            if (!humanAi.isEscaping)
            {
                humanAi.Escape();
            }
        }
    }

    Vector3 findOutRunAwayPointByDistance(int startIndex, float angleStep, int distanceScale)
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            float angle = angleStep * ((i+startIndex)%numberOfPoints);
            Vector2 point = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * checkRadius *distanceScale;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(point, checkRadius, layerMask);
            bool hit = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag(vineTag))
                {
                    hit = true;
                }
            }

            if (!hit)
            {
                return point;
            }
        }

        return Vector3.positiveInfinity;
    }
    
    private Vector3 RunAwayPoint()
    {
        float angleStep = 360f / numberOfPoints;
        int startIndex = Random.Range(0, numberOfPoints);
        for (int i = 2; i < 10; i += 2)
        {
            var point = findOutRunAwayPointByDistance(startIndex, angleStep, i);
            if (point.x != Vector3.positiveInfinity.x)
            {
                return point;
            }
        }

        var startAngle =startIndex*angleStep;
        return transform.position + new Vector3(Mathf.Cos(startAngle * Mathf.Deg2Rad), Mathf.Sin(startAngle * Mathf.Deg2Rad), 0) * checkRadius;
    }
    
    
    
    void RunAway()
    {

        //transform.position = RunAwayPoint();
        isRunningAway = true;
        var target = RunAwayPoint();
        var distance = (target - transform.position).magnitude;
        var time = distance * runAwayTime;
        transform.DOMove(RunAwayPoint(), time).SetEase(Ease.Linear);
        StartCoroutine(finishedRunAway(time));
    }

    
    
    IEnumerator finishedRunAway(float time)
    {
        yield return new WaitForSeconds(time);
        isRunningAway = false;
        
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
