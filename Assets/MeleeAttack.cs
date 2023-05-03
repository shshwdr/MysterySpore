using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class MeleeAttack : MonoBehaviour
{
    public float attackRange = 10f;
    public float attackInterval = 0.5f;
    private float damage = 0.3f;
    public float breakDelay = 2f;
    public bool isAttacking = false;
    private float nextAttackTime;
    private List<SpriteShapeController> spriteShapeControllers = new List<SpriteShapeController>();
    private Vector3 targetPosition;
    private float stopAttackTime = 3;
    private float stopAttackTimer = 0;
    private void Start()
    {
        
    }

    public void updateShapeControllers()
    {
        SpriteShapeController[] foundControllers = FindObjectsOfType<SpriteShapeController>();
        spriteShapeControllers = foundControllers.ToList();
    }

    private void Update()
    {
        
        
        if (stopAttackTimer > 0)
        {
            stopAttackTimer -= Time.deltaTime;
        }

        if (stopAttackTimer > 0)
        {
            return;
        }
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackInterval;
            PerformAttack();
        }

    }

    public bool  ClosestPosition(out Vector3  res)
    {
        
        res = Vector3.zero;
        if (stopAttackTimer > 0)
        {
            return false;
        }
        float minDistance = float.MaxValue;
        foreach (SpriteShapeController controller in spriteShapeControllers)
        {
            if (controller == null) continue;

            float distance;
            int index = GetClosestPointIndexOnSpriteShape(controller, transform.position, out distance);

            if (distance < minDistance )
            {
                minDistance = distance;
                res = controller.spline.GetPosition(index);
            }
        }

        if (minDistance < float.MaxValue)
        {
            GetComponent<HumanAI>().StopSeekPath();
            return true;
        }

        return false;
    }
    private void PerformAttack()
    {
        float minDistance = float.MaxValue;
        int closestIndex = -1;
        SpriteShapeController closestController = null;

        foreach (SpriteShapeController controller in spriteShapeControllers)
        {
            if (controller == null) continue;

            float distance;
            int index = GetClosestPointIndexOnSpriteShape(controller, transform.position, out distance);

            if (distance < minDistance && distance <= attackRange)
            {
                minDistance = distance;
                closestIndex = index;
                closestController = controller;
            }
        }

        if (closestController != null)
        {
            isAttacking = true;
            GetComponent<HumanAI>().StopSeekPath();
            ApplyDamage(closestController, closestIndex, damage);
        }
        else
        {
            isAttacking = false;
        }
    }

    private int GetClosestPointIndexOnSpriteShape(SpriteShapeController controller, Vector3 position, out float minDistance)
    {
        minDistance = float.MaxValue;
        int closestIndex = -1;
        Spline spline = controller.spline;

        for (int i = 0; i < spline.GetPointCount()-5; i++)
        {
            Vector3 pointWorldPosition = controller.transform.TransformPoint(spline.GetPosition(i));
            float distance = Vector3.Distance(position, pointWorldPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private void ApplyDamage(SpriteShapeController controller, int index, float damage)
    {
        // Apply damage logic here (e.g., decrease point's width and break the SpriteShape into two parts)
        // You can use "controller.spline.SetHeight(index, newHeight)" to adjust the width of the point at the specified index.

        // Example: Decrease the width by the damage value
        FloatingTextManager.Instance.addText("HIT!", transform.position, Color.red);
        float currentWidth = controller.GetComponent<SequentialWidthChange>().GetHeight(index);
        float newWidth = Mathf.Max(0, currentWidth - damage);
        controller.spline.SetHeight(index, newWidth);
        controller.GetComponent<SequentialWidthChange>().SetHeight(index,newWidth);

        if (newWidth <= 0)
        {
            // Break the SpriteShape into two parts
            // Note that this is a basic example and might not cover all scenarios.
            // You may need to handle edge cases and consider specific game mechanics.
            BreakSpriteShape(controller, index, breakDelay);
        }
    }

    private void BreakSpriteShape(SpriteShapeController controller, int index, float delay)
    {
        
        // Destroy the second part after the delay
        StartCoroutine(DestroySpriteShapePart(controller, index, delay));
    }
    private void RemoveAllVerticesAfter(Spline spline, int index)
    {
        int pointCount = spline.GetPointCount();
    
        for (int i = pointCount - 1; i > index; i--)
        {
            spline.RemovePointAt(i);
        }
    }
    private IEnumerator DestroySpriteShapePart(SpriteShapeController controller, int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (controller != null)
        {
            Spline spline = controller.spline;

            // Remove the point at the specified index
            //spline.RemovePointAt(index);

            RemoveAllVerticesAfter(spline, index-1);

            // Check if there are enough points to form a new SpriteShape
            int remainingPoints = spline.GetPointCount();
            if (remainingPoints < 3)
            {
                // If not, destroy the entire SpriteShapeController
                Destroy(controller.gameObject);
            }
            
            updateShapeControllers();
            stopAttackTimer = stopAttackTime;
            isAttacking = false;
            GetComponent<HumanAI>().FindNextRandomPath();
        }
    }
    
    
}