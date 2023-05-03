using System;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class RunAwayFromTarget : MonoBehaviour
{
    public float safeDistance = 5.0f;
    public float updateRate = 1.0f;
    private float updateTimer = 0;
    private Seeker seeker;
    //private IAstarAI ai;

    public  bool isRunningAway;

    private void Update()
    {
        isRunningAway = false;
        if (MPProgressManager.Instance.isStartingDraw)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer >= updateRate)
            {
                updateTimer = 0;

                
                UpdatePath();
            }
        }
    }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        //ai = GetComponent<IAstarAI>();

        // // Schedule the first update
        // InvokeRepeating("UpdatePath", 0f, updateRate);
    }

    void UpdatePath()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        // Check if the target is close enough
        if (Vector3.Distance(transform.position, mouseWorldPosition) < safeDistance)
        {
            
            isRunningAway = true;
            // Find a position away from the target
            // Vector3 direction = (transform.position - mouseWorldPosition).normalized;
            // Vector3 targetPosition = transform.position + direction * safeDistance;
            //
            // // Start a new path to the target position
            // seeker.StartPath(transform.position, targetPosition, GetComponent<HumanAI>(). OnPathComplete);
            StartCoroutine(FindNewPath());
        }
        else
        {
            // Stop the agent if the target is not close enough
            //ai.isStopped = true;
        }
    }
    
    public int maxAttempts = 10;
    public float searchRadius = 10.0f;
    System.Collections.IEnumerator FindNewPath()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random path that avoids obstacles
            Vector3 randomDirection = Random.insideUnitCircle.normalized * searchRadius;
            Vector3 targetPosition = transform.position + randomDirection;

            // Calculate the distance between the random position and the target
            float distanceToTarget = Vector3.Distance(targetPosition, mouseWorldPosition);

            // Check if the random position is far enough from the target
            if (distanceToTarget >= safeDistance)
            {
                // Start a new path to the target position
                seeker.StartPath(transform.position, targetPosition, GetComponent<HumanAI>().OnPathComplete);

                // Wait for the path calculation to complete
                while (seeker.IsDone() == false)
                {
                    yield return null;
                }
            }
        }
    }

    // void OnPathComplete(Path p)
    // {
    //     // If the path had an error, don't proceed
    //     if (p.error) return;
    //
    //     // Set the calculated path
    //     //ai.SetPath(p);
    //     //ai.isStopped = false;
    //     
    // }
}