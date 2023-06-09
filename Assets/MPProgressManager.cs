using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class MPProgressManager : Singleton<MPProgressManager>
{
    private HPBar hpbar;
    public float maxValue = 100;
    private float currentValue;

    public float startCost = 30;
    public float lungeCost = 10;
    public float moveCost = 1;
    public float recoverSpeed = 50;
    public float recoverFromHuman = 50;

    public bool unlimitEnergy = false;
    
    [HideInInspector]
    public bool isStartingDraw = false;
    private void Awake()
    {
        hpbar = GetComponent<HPBar>();
        currentValue = maxValue;
        hpbar.init(maxValue);
    }

    public bool CanLungeDistance()
    {
        
        if (unlimitEnergy)
        {
            return true;
        }

        bool res = currentValue > lungeCost;
        if (!res)
        {
            OutOfEnergy();
        }
        return res;
    }
    public bool CanDrawDistance(float dis)
    {
        
        if (unlimitEnergy)
        {
            return true;
        }
        currentValue -= moveCost*dis;
        updateValue();
        bool res = currentValue > 0;
        if (!res)
        {
            //startLunge();
            OutOfEnergy();
        }
        return res;
    }
    
    
    public void startLunge()
    {
        currentValue -= lungeCost;
        updateValue();
    }

    public bool CanStartDraw()
    {
        if (unlimitEnergy)
        {
            return true;
        }

        bool res = currentValue > startCost;
        if (!res)
        {
            startDraw();
            OutOfEnergy();
        }
        return res;
    }

    public void OutOfEnergy()
    {
        HumanManager.Instance.outOfEnergy();
    }

    public void startDraw()
    {
        isStartingDraw = true;
        currentValue -= startCost;
        updateValue();
    }

    void updateValue()
    {
        hpbar.updateCurrent(currentValue);
        
    }

    public void recoverEnergy(float value)
    {
        currentValue += value;
        currentValue = math.min(currentValue, maxValue);
        updateValue();
    }

    public void stopDraw()
    {
        isStartingDraw = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            unlimitEnergy = true;
        }
        if (!isStartingDraw)
        {
            if (GameManager.Instance.shouldRecoverEnergy())
            {
                currentValue += recoverSpeed * Time.deltaTime;
                currentValue = math.min(currentValue, maxValue);
            
                updateValue();
            }
        }
    }
}
