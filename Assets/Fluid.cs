using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Fluid : MonoBehaviour
{
    public float attackRange = 2;
    public float attackTime = 0.5f;
    public float stayTime = 3;
    private bool isDeactivated;
    public float damage = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destory",stayTime);
        InvokeRepeating("Damage",attackTime,attackTime);
    }

    void destory()
    {
        isDeactivated = true;
        GetComponent<Animator>().SetTrigger("deactivate");
        Destroy(gameObject,1f);
    }


    // Update is called once per frame
    void Update()
    {
    }

    void Damage()
    {
        if (isDeactivated)
        {
            return;
        }
        for(int v = 0;v<VinesManager.Instance.vines.Count;v++)
       // foreach (SpriteShapeController controller in VinesManager.Instance.vines)
        {
            var controller = VinesManager.Instance.vines[v];
            if (controller == null) continue;

            Spline spline = controller.spline;

            for (int i = 0; i < Math.Max(1,spline.GetPointCount()); i++)
            {
                Vector3 pointWorldPosition = controller.transform.TransformPoint(spline.GetPosition(i));
                float distance = Vector2.Distance(transform.position, pointWorldPosition);

                if (distance < attackRange)
                {
                    //do damage
                    
                    FloatingTextManager.Instance.addText("Hit!", controller.spline.GetPosition(i)+controller.transform.position, Color.green,0.5f);
                    var res = controller.GetComponent<VineLogic>().Damage(i, damage);
                    if (res)
                    {
                        v--;
                        //destory();
                        break;
                    }
                }
            }
        }
    }
}
