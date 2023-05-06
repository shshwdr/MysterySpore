using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public PolygonCollider2D defaultBoundSahpe;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
