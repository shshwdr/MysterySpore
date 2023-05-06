using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class VineLogic : MonoBehaviour
{
    private SpriteShapeController controller;
    public float breakDelay = 2f;
    public bool Damage(int index, float damage)
    {
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
            return true;
        }

        return false;
    }
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<SpriteShapeController>();
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

                controller.GetComponent<GameDraw>().DestorySelf() ;
                
                
            }else if (!controller.GetComponent<GameDraw>().isFinished)
            {
                controller.GetComponent<GameDraw>().finishCreation();
            }
            
        }
        
        yield return new WaitForSeconds(delay);
    }

}
