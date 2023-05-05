using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanManager : Singleton<HumanManager>
{
    public List<HumanAI> humans;
    public GameObject NextLevelOBj;
    
    // Start is called before the first frame update
    void Start()
    {
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
            NextLevelOBj.SetActive(true);
            NextLevelOBj.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.GotoNextLevel();
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
