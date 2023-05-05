using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        StartMenuMusic();
    }

    private void StartMenuMusic()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        //LevelManager.Instance.StartLatestLevel();
    }
}
