using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HumanManager : Singleton<HumanManager>
{
    public List<HumanAI> humans;

    public List<Knife> knifes;
   // public GameObject NextLevelOBj;
   public bool isWin = false;

   public bool isLose = false;
   public StartCinematic loseCinematic;
   public StartCinematic winCinematic;
   public void outOfEnergy()
   {
       if (isWin)
       {
           return;
       }

       StartCoroutine(lose());
   }

   IEnumerator lose()
   {
       
       if (isWin || isLose)
       {
           yield break;
       }

       if (!GameManager.Instance.hasDragged)
       {
           yield break;
       }
       yield return  StartCoroutine( DialogueManager.Instance.showLoseDialogue(GameManager.Instance.level == 0));
       if (GameManager.Instance.level == 0)
       {
           
       }
       else
       {
           isLose = true;
           AudioManager.Instance.PlayOneShot(FMODEvents.Instance.levelLose, transform.position);
           yield return StartCoroutine( loseCinematic.showLose());
           GameManager.Instance.restart();
       }
   }

   public void Win()
   {
       if (isWin || isLose)
       {
           return;
       }
       isWin = true;
       AudioManager.Instance.PlayOneShot(FMODEvents.Instance.levelComplete, transform.position);
       StartCoroutine(win());
   }
   IEnumerator win()
   {
       yield return  StartCoroutine( DialogueManager.Instance.showWinDialogue());

       {
           yield return StartCoroutine( winCinematic.showLose());
           GameManager.Instance.level = GameManager.Instance.maxLevel+1;
           GameManager.Instance.restart();
       }
   }
   
    // Start is called before the first frame update
    void Start()
    {
    }

    public void AddKnife(Knife knife)
    {
        knifes.Add(knife);
    }

    public void RemoveKnife(Knife knife)
    {
        knifes.Remove(knife);
    }

    public void AddHuman(HumanAI human)
    {
        humans.Add(human);
    }
    public void RemoveHuman(HumanAI human)
    {
        humans.Remove(human);
        if (humans.Count == 0)
        {
            isWin = true;
            GameObject.FindObjectOfType<Door>().Open();
            
            
            // NextLevelOBj.SetActive(true);
            // NextLevelOBj.GetComponent<Button>().onClick.AddListener(() =>
            // {
            //     GameManager.Instance.GotoNextLevel();
            // });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
