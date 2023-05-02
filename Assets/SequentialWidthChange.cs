using UnityEngine;
using UnityEngine.U2D;

public class SequentialWidthChange : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    public float widthMultiplier = 1.5f;
    public float changeSpeed = 1.0f;
    public float smoothness = 1.0f;

    private Spline spline;
    private int pointCount;
    private float[] originalHeights;
    private float elapsedTime;
    private int currentIndex;
    private bool finished = false;

    public void SetHeight(int index, float value)
    {
        originalHeights[index] = value;
    }
    private void Start()
    {
        spriteShapeController = GetComponent<SpriteShapeController>();
        spline = spriteShapeController.spline;
    }

    public void FinishCreation()
    {
        finished = true;
        pointCount = spline.GetPointCount();
        originalHeights = new float[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            originalHeights[i] = spline.GetHeight(i);
        }
    }

    private void Update()
    {
        if (finished&& pointCount > 0 && originalHeights != null )
        {
            
            elapsedTime += Time.deltaTime;

            if (elapsedTime > changeSpeed)
            {
                elapsedTime = 0;
                currentIndex = (currentIndex + 1) % pointCount;
                
                
                for (int i = 0; i < pointCount; i++)
                {
                    float distanceFromCurrent = Mathf.Abs(i - currentIndex);
                    float multiplier = GaussianFunction(distanceFromCurrent, smoothness) * (widthMultiplier - 1.0f) + 1.0f;
                    float newHeight = originalHeights[i] * multiplier;
                    if (i < spline.GetPointCount())
                    {
                        spline.SetHeight(i, newHeight);
                    }
                }

                spriteShapeController.RefreshSpriteShape();
            }

            
            
            
            // elapsedTime += Time.deltaTime;
            //
            // if (elapsedTime > changeSpeed)
            // {
            //     elapsedTime = 0;
            //     spline.SetHeight(currentIndex, originalHeights[currentIndex]);
            //     currentIndex = (currentIndex + 1) % pointCount;
            //
            //     float newHeight = originalHeights[currentIndex] * widthMultiplier;
            //     spline.SetHeight(currentIndex, newHeight);
            //
            //     spriteShapeController.RefreshSpriteShape();
            // }
        }
    }

    private float GaussianFunction(float x, float smoothness)
    {
        return Mathf.Exp(-x * x / (2 * smoothness * smoothness));
    }
}