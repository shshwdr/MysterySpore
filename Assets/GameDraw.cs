using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;
public class GameDraw : MonoBehaviour
{
    public float minimumDistance = 1.0f;
    public Vector3 lastPosition;
    private Vector3 startPosition;
    public bool isFinished = true;

    public float heightStart = 1.4f;

    public float heightDecrease = 0.01f;
    public float heightMin = 0.7f;

    
    
    public float growTime = 0.3f;

    private float growTimer = 0;

    public List<GameDraw> children = new List<GameDraw>();
    public GameDraw parent;
    
    

    public float growLength = 1;

     void RemoveChildren()
    {
        Debug.Log("remove children "+children.Count);
        for (int i = 0; i < children.Count; i++)
        {
            var child = children[0];
            if (!child)
            {
            }
            else
            {
                child.DestorySelf();
            }
            children.Remove(child);
        }
    }

    public void DestorySelf()
    {
        RemoveChildren();
        
        VinesManager.Instance.removeVine(GetComponent<SpriteShapeController>());
        Destroy(gameObject);

        if (!isFinished)
        {
            finishCreation();
        }
    }
   public void init(Vector3 startPos,Vector3 lastP,float width, GameDraw parent)
    {
        isFinished = false;
        lastPosition = lastP;
        startPosition = startPos;
        heightStart = width;
        this.parent = parent;
        if (this.parent)
        {
            parent.children.Add(this);
        }
    }
    // Use this for initialization
    void Start()
    {
    }

    private static int NextIndex(int index, int pointCount)
    {
        return Mod(index + 1, pointCount);
    }

    private static int PreviousIndex(int index, int pointCount)
    {
        return Mod(index - 1, pointCount);
    }

    private static int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    private void Smoothen(SpriteShapeController sc, int pointIndex)
    {
        Vector3 position = sc.spline.GetPosition(pointIndex);
        Vector3 positionNext = sc.spline.GetPosition(NextIndex(pointIndex, sc.spline.GetPointCount()));
        Vector3 positionPrev = sc.spline.GetPosition(PreviousIndex(pointIndex, sc.spline.GetPointCount()));
        Vector3 forward = gameObject.transform.forward;

        float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

        Vector3 leftTangent = (positionPrev - position).normalized * scale;
        Vector3 rightTangent = (positionNext - position).normalized * scale;

        sc.spline.SetTangentMode(pointIndex, ShapeTangentMode.Continuous);
        SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent,
            out leftTangent);

        sc.spline.SetLeftTangent(pointIndex, leftTangent);
        sc.spline.SetRightTangent(pointIndex, rightTangent);
    }

    void finishCreation()
    {
        
        isFinished = true;
            
        //GetComponent<Sprinkle>().UpdateSprinkles();
        GetComponent<SequentialWidthChange>().FinishCreation();
        MPProgressManager.Instance.stopDraw();
        // foreach (var humanAi in HumanManager.Instance.humans)
        // {
        //     if (humanAi && humanAi.GetComponentInChildren<MeleeAttack>())
        //     {
        //         humanAi.GetComponentInChildren<MeleeAttack>().updateShapeControllers();
        //     }
        // }
        //
        VinesManager.Instance.addVine(GetComponent<SpriteShapeController>());
        
        GameObject.FindObjectOfType<AstarPath>().Scan();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            finishCreation();
        }

        growTimer += Time.deltaTime;
        if (growTimer > growTime)
        {
            growTimer = 0;
            
            var mp = Input.mousePosition;
            mp.z = 10.0f;
            mp = Camera.main.ScreenToWorldPoint(mp);
            mp -= startPosition;
            var dt = Mathf.Abs((mp - lastPosition).magnitude);
            var md = minimumDistance;
            if (Input.GetMouseButton(0) && dt > md)
            {
                var spriteShapeController = gameObject.GetComponent<SpriteShapeController>();
                var spline = spriteShapeController.spline;
                var dir = (mp - lastPosition).normalized *growLength * MouseController.Instance.speed;
            
                spline.InsertPointAt(spline.GetPointCount(), lastPosition+dir);
                var newPointIndex = spline.GetPointCount() - 1;
                Smoothen(spriteShapeController, newPointIndex - 1);

                spline.SetHeight(newPointIndex, heightStart/*UnityEngine.Random.Range(0.9f, 1.1f)*/);
                heightStart -= heightDecrease;
                
                lastPosition = lastPosition+dir;

                GameObject.FindObjectOfType<AstarPath>().Scan();

                // foreach (var humanAi in HumanManager.Instance.humans)
                // {
                //     humanAi.FindNextRandomPath();
                // }
                if (heightStart <= heightMin)
                {
                    
                    finishCreation();
                }
                if (!MPProgressManager.Instance.CanDrawDistance(growLength* MouseController.Instance.cost))
                {
                
                    finishCreation();
                }
            }
        }
        
        
    }
}