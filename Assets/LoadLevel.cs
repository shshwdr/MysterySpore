using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public GameObject SmallGrid;
    public GameObject BigGrid;
    public PolygonCollider2D defaultBoundSahpe;

    private void Awake()
    {
        if (GameManager.Instance.level < 6 || GameManager.Instance.level>7)
        {
            SmallGrid.SetActive(true);
        }
        else
        {
            
            BigGrid.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        var levelOB = transform.Find(GameManager.Instance.level.ToString());
        if (levelOB)
        {
            levelOB.gameObject.SetActive(true);
            if (levelOB.GetComponentInChildren<PolygonCollider2D>())
            {
                GameObject.FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D =
                    levelOB.GetComponentInChildren<PolygonCollider2D>();
            }
            else
            {
                GameObject.FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = defaultBoundSahpe;
            }
        }

        if (GameManager.Instance.level == 0)
        {
            GameManager.Instance.hasDragged = false;
            GameManager.Instance.hasKilled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}