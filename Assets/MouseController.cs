using UnityEngine;
using UnityEngine.U2D;

public class MouseController : MonoBehaviour
{
    public SpriteShapeController[] spriteShapeControllers;
    public int numberOfSamples = 100;
    public GameObject shapePrefab;
    private float growDistance =5;

    public Transform Core;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!MPProgressManager.Instance.CanStartDraw())
            {
                FloatingTextManager.Instance.addText("Not Enough Energy", Vector3.zero, Color.black);
                return;
            }
            spriteShapeControllers = GameObject.FindObjectsOfType<SpriteShapeController>();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            float closestDistance = Mathf.Infinity;
            Vector3 closestPoint = Vector3.zero;

            var corePoint = Core.position;
            var dis = Vector3.Distance(mouseWorldPosition, corePoint);
            if (dis < closestDistance)
            {
                closestDistance = dis;
                closestPoint = corePoint;
            }

            foreach (var spriteShapeController in spriteShapeControllers)
            {
                for (int i = 0; i < numberOfSamples; i++)
                {
                    float t = (float)i / numberOfSamples;
                    Vector3 point = GetInterpolatedPosition(spriteShapeController.spline, t) + spriteShapeController.transform.position;
                    float distance = Vector3.Distance(mouseWorldPosition, point);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPoint = point;
                    }
                }
            }

            if (closestDistance < growDistance)
            {
                
                var go = Instantiate(shapePrefab);
                go.transform.position = closestPoint;
                go.GetComponent<SpriteShapeController>().spline.SetPosition(0,Vector3.zero);
                go.GetComponent<SpriteShapeController>().spline.SetPosition(1,mouseWorldPosition - closestPoint);
                go.GetComponent<GameDraw>().init(closestPoint, mouseWorldPosition - closestPoint);
                MPProgressManager.Instance.startDraw();
            }
        }

        
    }

    private Vector3 GetInterpolatedPosition(Spline spline, float t)
    {
        int pointCount = spline.GetPointCount();
        float interpolatedIndex = t * (pointCount - 1);
        int startIndex = Mathf.FloorToInt(interpolatedIndex);
        int endIndex = Mathf.CeilToInt(interpolatedIndex);

        if (startIndex == endIndex)
        {
            return spline.GetPosition(startIndex);
        }

        Vector3 startPosition = spline.GetPosition(startIndex);
        Vector3 endPosition = spline.GetPosition(endIndex);
        float localT = interpolatedIndex - startIndex;

        return Vector3.Lerp(startPosition, endPosition, localT);
    }
}