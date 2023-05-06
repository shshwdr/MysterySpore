using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool shouldSkipCinematic = false;
    public int level = 0;
    
    public bool hasKilled = false;
    public bool hasDragged = false;

    public int maxLevel = 6;
    public bool skipCinematic()
    {
        return shouldSkipCinematic || level != 0;
    }

    public void  restart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GotoNextLevel()
    {
        if (level >= maxLevel)
        {
            HumanManager.Instance.Win();
        }
        else
        {
            level++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }public void GotoPrevLevel()
    {
        if (level <= 0)
        {
            return;
        }
        level--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //StartCoroutine(DialogueManager.Instance.showTutorialDialogue(0));
    }

    public bool shouldRecoverEnergy()
    {
        return level == 0 || HumanManager.Instance.isWin || level>maxLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GotoNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GotoPrevLevel();
        }
    }
}
