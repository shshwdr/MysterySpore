using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class MeleeAttack : HumanAttack
{
    public float attackRange = 10f;
    public float damage = 0.3f;
    private Vector3 targetPosition;




    
    protected override void PerformAttack()
    {
        float minDistance = float.MaxValue;
        int closestIndex = -1;
        SpriteShapeController closestController = null;

        foreach (SpriteShapeController controller in VinesManager.Instance.vines)
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

        if (closestController != null && closestIndex<closestController.spline.GetPointCount()-1)
        {
            isAttacking = true;
            GetComponent<HumanAI>().isMoving = false;
            GetComponent<HumanAI>().StopSeekPath();
            ApplyDamage(closestController, closestIndex, damage);
        }
        else
        {
            isAttacking = false;
        }
    }


    
}