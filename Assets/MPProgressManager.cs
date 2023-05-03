using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MPProgressManager : Singleton<MPProgressManager>
{
    private HPBar hpbar;
    private float maxValue = 100;
    private float currentValue;

    private float startCost = 30;
    private float moveCost = 1;
    public bool isStartingDraw = false;
    private float recoverSpeed = 50;
    
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
