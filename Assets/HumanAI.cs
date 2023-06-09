using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class HumanAI : MonoBehaviour
{


    public float speed = 200f;


    private Rigidbody2D rb;

    private Human human;
    public float moveRange = 10f;
    public float minTimeBetweenMoves = 1f;
    public float maxTimeBetweenMoves = 3f;
    public float nextPointDistanceThreshold = 0.5f;

    public Seeker seeker;
    private Path path;
    private int currentWaypoint;
    private Animator animator;

    public bool isMoving = false;
    public bool isEscaping = false;
    private AstarPath astar;
    public  bool isRunningAway; //is he is actually running away
    public bool hasTube = false;
    public GameObject fluidPrefab;
    public float fluidGenerateTime = 0.3f;
    private float fluidGenerateTimer;
    public AnimatorOverrideController meleeAnimatorController;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        astar = AstarPath.active;
        if (astar == null)
        {
            Debug.LogError("No active AstarPath found in the scene.");
            return;
        }
        //FindNextRandomPath();
    }

    private void Update()
    {
        if (path == null)
        {
            isRunningAway = false;
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) //reach the end of the current path
        {
            isMoving = false;
            FindNextRandomPath();
            isEscaping = false;
            isRunningAway = false;
            return;
        }

        // if (GetComponent<RunAwayFromTarget>().isRunningAway)
        // {
        //     return;
        // }

        

        if (hasTube)
        {
            
            fluidGenerateTimer += Time.deltaTime;
            if (fluidGenerateTimer >= fluidGenerateTime)
            {
                fluidGenerateTimer = 0;
                Instantiate(fluidPrefab, transform.position, Quaternion.identity);
            }
        }
        //actual move code with animation
        Vector3 direction = ((Vector2)(path.vectorPath[currentWaypoint] - transform.position)).normalized;
        animator.SetTrigger("move");
        animator.SetFloat("horizontal",direction.x);
        animator.SetFloat("verticle",direction.y);
        float distanceToWaypoint = Vector2.Distance((Vector2)transform.position, (Vector2)path.vectorPath[currentWaypoint]);

        if (distanceToWaypoint < nextPointDistanceThreshold)
        {
            currentWaypoint++;
        }
        else
        {
            //rb.MovePosition((Vector3)rb.position+(direction * (Time.deltaTime * (human.isSuffering ? speed / 2:speed))));
            transform.position += direction * Time.deltaTime * (human.isSuffering ? speed / 2:speed);
        }
    
        if (!hasTube &&( !GetComponent<HumanAttack>() || !GetComponent<HumanAttack>().enabled))
        {
            foreach (var knife in HumanManager.Instance.knifes)
            {
                if ((knife.transform.position - transform.position).magnitude <= collectDistance)
                {
                    
                    AudioManager.Instance.PlayOneShot(FMODEvents.Instance.knifeEquip, transform.position);
                    //equip knife
                    knife.destory();

                    GetComponent<MeleeAttack>().enabled = true;

                    animator.runtimeAnimatorController = meleeAnimatorController;
                    animator.Rebind();
                    path = null;
                    isMoving = false;
                    break;
                }
            }
        }
    }


    // public void Escape()
    // {
    //     isEscaping = true;
    //     FindNextRandomPath();
    // }
    public void FindNextRandomPath()
    {

        if (!astar)
        {
            return;
        }
        if (GetComponent<RunAwayFromTarget>().shouldRunAway())
        {
            GetComponent<RunAwayFromTarget>().UpdatePath();
        }
        
        if (GetComponent<HumanAttack>() && GetComponent<HumanAttack>().enabled)
        {
            if (GetComponent<HumanAttack>().isAttacking)
            {
                return;
            }
            //if is attacker, find attackable point and move
            Vector3 res;
            var meleeFoundTarget = GetComponent<HumanAttack>().ClosestPosition(out res);
            if (meleeFoundTarget)
            {
                //GraphNode startNode = astar.GetNearest(transform.position).node;
                //GraphNode endNode = astar.GetNearest(res).node;
                //bool isPathPossible = PathUtilities.IsPathPossible(startNode, endNode);

                //if (isPathPossible)
                {
                    seeker.StartPath(transform.position, res, OnPathComplete);
                    return;
                }
            }
        }
        else if(!hasTube)
        {
            //if there is knife nearby, fetch it. 
            foreach (var knife in HumanManager.Instance.knifes)
            {
                if ((knife.transform.position - transform.position).magnitude <= knifeSearchDistance)
                {
                   // GraphNode startNode = astar.GetNearest(transform.position).node;
                   // GraphNode endNode = astar.GetNearest(knife.transform.position).node;
                    //bool isPathPossible = PathUtilities.IsPathPossible(startNode, endNode);
                    //if (isPathPossible)
                    {
                        seeker.StartPath(transform.position, knife.transform.position, OnPathComplete);
                    }
                }
            }
            
        }

        return;
        // {
        //     Vector3 randomPosition = GetRandomPositionAwayFromTarget(Vector3.zero, 10, 50);
        //     //Vector3 randomPosition = GetRandomPosition(transform.position, moveRange);
        //     seeker.StartPath(transform.position, randomPosition, OnPathComplete);
        // }
    }

    private float collectDistance = 5;
    private float knifeSearchDistance = 10;

    public void StopSeekPath()
    {
        seeker.CancelCurrentPathRequest();
        path = null;
    }
    
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            isMoving = true;
        }
        else
        {
            Debug.Log("path error");
        }
    }

    // private Vector3 GetRandomPosition(Vector3 center, float range)
    // {
    //     float x = Random.Range(center.x - range, center.x + range);
    //     float z = Random.Range(center.z - range, center.z + range);
    //
    //     return new Vector3(x, z, 0);
    // }
    //
    // private Vector3 GetRandomPositionAwayFromTarget(Vector3 targetPosition, float minRange, float maxRange)
    // {
    //     Vector3 randomDirection = Random.insideUnitSphere.normalized;
    //     float randomDistance = Random.Range(minRange, maxRange);
    //
    //     Vector3 randomPosition = targetPosition + randomDirection * randomDistance;
    //     randomPosition.z = targetPosition.z;
    //
    //     return randomPosition;
    // }

}
