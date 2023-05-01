using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image frontImage;

    private float maxValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(float maxHP)
    {
        maxValue = maxHP;
        frontImage.fillAmount = 1;
    }

    public void updateCurrent(float hp)
    {
        frontImage.fillAmount = hp / maxValue;
    }
}
