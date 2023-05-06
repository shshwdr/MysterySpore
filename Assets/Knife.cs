using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HumanManager.Instance.AddKnife(this);
    }

    public void destory()
    {
        HumanManager.Instance.RemoveKnife(this);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
