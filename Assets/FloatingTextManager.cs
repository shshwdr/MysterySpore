using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : Singleton<FloatingTextManager>
{
    public GameObject floatingTextObj;


    public FloatingText addText(string text, Vector3 pos, Color color,float scale = 1, bool autoDispose = true, float stayTime = 0.4f)
    {
        var obj = Instantiate(floatingTextObj);
        obj.SetActive(true);
        FloatingText floatingText = obj.GetComponent<FloatingText>();
        floatingText.init(text, pos, color, stayTime, autoDispose);
        floatingText.transform.localScale = Vector3.one *scale;
        return floatingText;
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
