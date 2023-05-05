using System;
using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RunAwayFromTarget : MonoBehaviour
{
    public float safeDistance = 5.0f;
     float updateTime = 0.1f;
    private float updateTimer = 0;
    private Seeker seeker;

    private AstarPath astar;
    private bool firstTime = true;
    private void Update()
    {
        //isRunningAway = false;
        updateTimer += Time.deltaTime;
        if (GetComponent<HumanAI>().isMoving)
        {
            return;
        }
        if (MPProgressManager.Instance.isStartingDraw)
        {
            if (updateTimer >= updateTime)
            {
                updateTimer = 0;
                UpdatePath();
            }
        }
    }

    void Awake()
    {
        seeker = GetComponent<Seeker>();
        updateTimer = updateTime;
    }

    private void Start()
    {
        
        astar = AstarPath.active;
        if (astar == null)
        {
            Debug.LogError("No active AstarPath found in the scene.");
            return;
        }
    }

    public bool shouldRunAway()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        // Check if the mouse is close enough
        return (Vector3.Distance(transform.position, mouseWorldPosition) < safeDistance);
    }
    public void UpdatePath()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        // Check if the mouse is close enough
        if (Vector3.Distance(transform.position, mouseWorldPosition) < safeDistance)
        {
            //isRunningAway = true;
            // Find a position away from the target
            // Vector3 direction = (transform.position - mouseWorldPosition).normalized;
            // Vector3 targetPosition = transform.position + direction * safeDistance;
            //
            // // Start a new path to the target position
            // seeker.StartPath(transform.position, targetPosition, GetComponent<HumanAI>(). OnPathComplete);
            //StartCoroutine(FindNewPath());
            if (firstTime)
            {
                firstTime = false;
                if (GetComponent<ShowText>())
                {
                    GetComponent<ShowText>().Show();
                }
            }
            FindNewPath();
        }
        else
        {
            // Stop the agent if the target is not close enough
            //ai.isStopped = true;
        }
    }
    
    public int maxAttempts = 10;
    public float searchRadius = 10.0f;
    
    void FindNewPath()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        var step = (searchRadius - safeDistance) / 3;
        for (float dis = safeDistance; dis < searchRadius; dis += step)
        {
            if (findPath(dis, mouseWorldPosition,true))
            {
                return;
            }
        }

        for (float dis = safeDistance; dis > 0; dis -= step)
        {
            if (findPath(dis, mouseWorldPosition,false))
            {
                return;
            }
        }
        //still not find any path, them just run to anywhere man! dont' stop
    }

    bool findPath(float radius,Vector3 mouseWorldPosition,bool needFar)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random path that avoids obstacles
            Vector3 randomDirection = Random.insideUnitCircle.normalized * radius;
            Vector3 targetPosition = transform.position + randomDirection;

            // Calculate the distance between the random position and the target
            float distanceToTarget = Vector3.Distance(targetPosition, mouseWorldPosition);

            // Check if the random position is far enough from the target
            if (!needFar || distanceToTarget >= safeDistance)
            {
                GraphNode startNode = astar.GetNearest(transform.position).node;
                GraphNode endNode = astar.GetNearest(targetPosition).node;
                bool isPathPossible = PathUtilities.IsPathPossible(startNode, endNode);
                if (isPathPossible)
                {
                    // Start a new path to the target position
                    seeker.StartPath(transform.position, targetPosition, GetComponent<HumanAI>().OnPathComplete);
                    GetComponent<HumanAI>(). isRunningAway = true;
                    return true;
                }

            }
        }

        return false;
    }
    
    
}