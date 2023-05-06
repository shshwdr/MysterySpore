using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class HumanAttack : MonoBehaviour
{
    
    //public float stopAttackTime = 3;
    //private float stopAttackTimer = 0;
    public bool isAttacking = false;
    
    private float nextAttackTime;
    public float attackInterval = 0.5f;

    public bool wouldAttack()
    {
        return true;
        // return stopAttackTimer <= 0;
    }
    
    
    private void Update()
    {
        // if (stopAttackTimer < 0  && !isAttacking && GetComponent<HumanAI>().seeker.IsDone())
        // {
        //     GetComponent<HumanAI>().FindNextRandomPath();
        // }
        
        // if (stopAttackTimer > 0)
        // {
        //     stopAttackTimer -= Time.deltaTime;
        // }
        //
        // if (stopAttackTimer > 0)
        // {
        //     return;
        // }

        if (GetComponent<HumanAI>().isRunningAway)
        {
            return;
        }

        if (DialogueManager.Instance.isInDialogue)
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
        if (!wouldAttack())
        {
            return false;
        }
        float minDistance = float.MaxValue;
        foreach (SpriteShapeController controller in VinesManager.Instance.vines)
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
    
    protected int GetClosestPointIndexOnSpriteShape(SpriteShapeController controller, Vector3 position, out float minDistance)
    {
        minDistance = float.MaxValue;
        int closestIndex = -1;
        Spline spline = controller.spline;

        for (int i = 1; i < Math.Max(1,spline.GetPointCount()-5); i++)
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
    protected virtual void PerformAttack()
    {
    }

    private bool firstTime;
    protected void ApplyDamage(SpriteShapeController controller, int index, float damage)
    {
        if (firstTime)
        {
            var suc = DialogueManager.Instance.showPopup(GetComponent<ShowText>(), " Attack");
            if (suc)
            {
                firstTime = false;
            }
        }

        // if (firstTime)
        // {
        //     firstTime = false;
        //     if (Random.Range(0, 100) > 50)
        //     {
        //         
        //         if (GetComponent<ShowText>())
        //         {
        //             GetComponent<ShowText>().Show(" Attack");
        //         }
        //     }
        // }
        // Apply damage logic here (e.g., decrease point's width and break the SpriteShape into two parts)
        // You can use "controller.spline.SetHeight(index, newHeight)" to adjust the width of the point at the specified index.

        // Example: Decrease the width by the damage value

        if (GetComponent<Human>().animator)
        {
            
            
            var direction = controller.spline.GetPosition(index)+controller.transform.position - transform.position;
            
            
            GetComponent<Human>().animator.SetFloat("horizontal",direction.x);
            GetComponent<Human>().animator.SetFloat("verticle",direction.y);
            
            GetComponent<Human>().animator.SetTrigger("attack");

            
        }


        FloatingTextManager.Instance.addText("HIT!", controller.spline.GetPosition(index)+controller.transform.position, Color.red);
        var res = controller.GetComponent<VineLogic>().Damage(index, damage);
        if (res)
        {
            isAttacking = false;
            GetComponent<HumanAI>().FindNextRandomPath();
        }
    }

    

}
