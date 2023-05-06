using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AutoGeneration : MonoBehaviour
{
    public GameObject[] prefabs;

    private float generateTime = 1;

    private float generateTimer = 0;

    private float maxCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public Vector2 FindWalkablePosition()
    {
        // Get the current graph
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        // Try a limited number of times to find a walkable position
        int maxAttempts = 100;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            // Generate random coordinates
            int x = Random.Range(0, gridGraph.width);
            int z = Random.Range(0, gridGraph.depth);

            // Get the node at the random coordinates
            GridNodeBase node = gridGraph.GetNode(x, z);

            // Check if the node is walkable
            if (node != null && node.Walkable)
            {
                // Convert the node position to world space and return it
                Vector3 worldPosition = (Vector3)gridGraph.GraphPointToWorld(x, z, node.position.y);
                return new Vector2(worldPosition.x, worldPosition.y);
            }
        }

        // If a walkable position is not found, return a default position
        return Vector2.zero;
    }





    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.isInDialogue)
        {
            return;
        }
        generateTimer += Time.deltaTime;
        if (generateTimer >= generateTime && HumanManager.Instance.humans.Count<=maxCount)
        {
            generateTimer = 0;

            var findPosition = FindWalkablePosition();
            Instantiate(prefabs[Random.Range(0, prefabs.Length)], findPosition, Quaternion.identity,transform);
        }
    }
}
