using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class HumanAI : MonoBehaviour
{

    public Vector3 targetPosition;

    public float speed = 200f;

    public float nextWaypointDistance = 3f;

    // private Path path;
    //
    // private int currentWaypoint = 0;
    //
    // private bool reachedEndOfPath = false;
    //
    // private Seeker seeker;

    private Rigidbody2D rb;

    // private void Awake()
    // {
    //     seeker = GetComponent<Seeker>();
    //     rb = GetComponent<Rigidbody2D>();
    //     seeker.StartPath(rb.position,)
    // }
    private Human human;
    public float moveRange = 10f;
    public float minTimeBetweenMoves = 1f;
    public float maxTimeBetweenMoves = 3f;
    public float nextPointDistanceThreshold = 0.5f;

    public Seeker seeker;
    private Path path;
    private int currentWaypoint;
    private float nextMoveTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        nextMoveTime = Time.time + Random.Range(minTimeBetweenMoves, maxTimeBetweenMoves);
        FindNextRandomPath();
    }

    private void Update()
    {

        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            
            FindNextRandomPath();
            isEscaping = false;
        return;
        }

        if (GetComponent<RunAwayFromTarget>().isRunningAway)
        {
            return;
        }

        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        float distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distanceToWaypoint < nextPointDistanceThreshold)
        {
            currentWaypoint++;
        }
        else
        {
            //rb.MovePosition((Vector3)rb.position+(direction * (Time.deltaTime * (human.isSuffering ? speed / 2:speed))));
            transform.position += direction * Time.deltaTime * (human.isSuffering ? speed / 2:speed);
        }
    }

    public bool isEscaping = false;

    public void Escape()
    {
        isEscaping = true;
        FindNextRandomPath();
    }
    public void FindNextRandomPath()
    {
        
        if (GetComponent<HumanAttack>())
        {
            if (GetComponent<HumanAttack>().isAttacking)
            {
                return;
            }

            if (!seeker.IsDone())
            {
                return;
            }
            Vector3 res;
            var meleeFoundTarget = GetComponent<HumanAttack>().ClosestPosition(out res);
            if (meleeFoundTarget)
            {
                seeker.StartPath(transform.position, res, OnPathComplete);
                return;
            }
        }

        // if (GetComponent<ShootAttack>())
        // {
        //     if (GetComponent<MeleeAttack>().isAttacking)
        //     {
        //         return;
        //     }
        // }
        return;
        {
            Vector3 randomPosition = GetRandomPositionAwayFromTarget(Vector3.zero, 10, 50);
            //Vector3 randomPosition = GetRandomPosition(transform.position, moveRange);
            seeker.StartPath(transform.position, randomPosition, OnPathComplete);
        }
    }

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
        }
    }

    private Vector3 GetRandomPosition(Vector3 center, float range)
    {
        float x = Random.Range(center.x - range, center.x + range);
        float z = Random.Range(center.z - range, center.z + range);

        return new Vector3(x, z, 0);
    }
    
    private Vector3 GetRandomPositionAwayFromTarget(Vector3 targetPosition, float minRange, float maxRange)
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        float randomDistance = Random.Range(minRange, maxRange);

        Vector3 randomPosition = targetPosition + randomDirection * randomDistance;
        randomPosition.z = targetPosition.z;

        return randomPosition;
    }

}
