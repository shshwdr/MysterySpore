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

    public float moveRange = 10f;
    public float minTimeBetweenMoves = 1f;
    public float maxTimeBetweenMoves = 3f;
    public float nextPointDistanceThreshold = 0.5f;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint;
    private float nextMoveTime;

    private void Start()
    {
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

        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        float distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distanceToWaypoint < nextPointDistanceThreshold)
        {
            currentWaypoint++;
        }
        else
        {
            transform.position += direction * Time.deltaTime * speed;
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
        Vector3 randomPosition = GetRandomPosition(transform.position, moveRange);
        seeker.StartPath(transform.position, randomPosition, OnPathComplete);
    }
    private void OnPathComplete(Path p)
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

}
