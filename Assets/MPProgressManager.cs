using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MPProgressManager : Singleton<MPProgressManager>
{
    private HPBar hpbar;
    public float maxValue = 100;
    private float currentValue;

    public float startCost = 30;
    public float moveCost = 1;
    public float recoverSpeed = 50;
    public float recoverFromHuman = 50;
    
    
    [HideInInspector]
    public bool isStartingDraw = false;
    private void Awake()
    {
        hpbar = GetComponent<HPBar>();
        currentValue = maxValue;
        hpbar.init(maxValue);
    }

    public bool CanDrawDistance(float dis)
    {
        
        currentValue -= moveCost*dis;
        updateValue();
        return currentValue > 0;
    }

    public bool CanStartDraw()
    {
        return currentValue > startCost;
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
        if (!isStartingDraw)
        {
            currentValue += recoverSpeed * Time.deltaTime;
            currentValue = math.min(currentValue, maxValue);
            
            updateValue();
        }
    }
}
