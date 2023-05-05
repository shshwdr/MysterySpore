using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.SetMusic(AudioManager.LevelTheme.MAIN_MENU);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
