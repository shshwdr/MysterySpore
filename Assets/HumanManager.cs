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

   public void outOfEnergy()
   {
       if (isWin)
       {
           return;
       }

       isLose = true;
       StartCoroutine(lose());
   }

   IEnumerator lose()
   {
       yield return  StartCoroutine( DialogueManager.Instance.showLoseDialogue(GameManager.Instance.level == 0));
       if (GameManager.Instance.level == 0)
       {
           
       }
       else
       {
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
