using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUISound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playUISound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.uiClick, transform.position);
    }
}
