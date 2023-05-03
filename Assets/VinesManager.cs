using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class VinesManager : Singleton<VinesManager>
{
    
    public List<SpriteShapeController> vines = new List<SpriteShapeController>();

    public void addVine(SpriteShapeController vine)
    {
        vines.Add(vine);
    }

    public void removeVine(SpriteShapeController vine)
    {
        
        vines.Remove(vine);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
