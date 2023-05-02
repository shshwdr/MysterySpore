using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : Singleton<HumanManager>
{
    public List<HumanAI> humans;
    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var human in GameObject.FindObjectsOfType<HumanAI>())
        {
            humans.Add(human);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
