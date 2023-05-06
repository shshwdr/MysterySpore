using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ShootAttack : HumanAttack
{
    public float attackRange = 10f;
    public float damage = 0.3f;
    private Vector3 targetPosition;
    private LaserLine line;
    public Transform laserLineStart;
    
    protected override void PerformAttack()
    {

        if (!GetComponent<Robot>().isActivated || Time.time -  GetComponent<Robot>().activateTime<1)
        {
            return;
        }
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
            GetComponent<HumanAI>().StopSeekPath();
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.robotRays, transform.position);

            ApplyDamage(closestController, closestIndex, damage);
            
            line.startPoint = laserLineStart;
            if (closestIndex >= closestController.spline.GetPointCount())
            {
                isAttacking = false;
                return;
            }
            line.endPoint = closestController.transform.TransformPoint(closestController.spline.GetPosition(closestIndex));
            line.gameObject.SetActive(true);
            StartCoroutine(hideLine());
        }
        else
        {
            isAttacking = false;
        }
    }

    private void Awake()
    {
        line = GetComponentInChildren<LaserLine>(true);
    }

    IEnumerator hideLine()
    {
        yield return new WaitForSeconds(0.1f);
        line.gameObject.SetActive(false);
    }
}
