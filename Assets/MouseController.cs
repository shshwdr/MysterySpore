using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class MouseController : Singleton<MouseController>
{
    public SpriteShapeController[] spriteShapeControllers;
    public int numberOfSamples = 100;
    public GameObject shapePrefab;
    public float growDistance =5;

    public Transform Core;
    public bool boost;
    private float boostScale = 2;
    private float boostCostScale = 2;
    public float speed => boost ? boostScale : 1;
    public float cost => boost ? boostCostScale : 1;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            boost = true;
        }
        else
        {
            boost = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
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
            float width = 1;
            var corePoint = Core.position;
            var dis = Vector3.Distance(mouseWorldPosition, corePoint);
            if (dis < closestDistance)
            {
                closestDistance = dis;
                closestPoint = corePoint;
            }

            SpriteShapeController closestController = null;
            foreach (var spriteShapeController in spriteShapeControllers)
            {
                for (int i = 0; i < spriteShapeController.spline.GetPointCount(); i++)
                {
                    Vector3 point = spriteShapeController.spline.GetPosition(i) + spriteShapeController.transform.position;
                    float distance = Vector3.Distance(mouseWorldPosition, point);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPoint = point;
                        width = spriteShapeController.spline.GetHeight(i);
                        closestController = spriteShapeController;
                    }
                }
            }

            if (closestDistance < growDistance)
            {
                
                var go = Instantiate(shapePrefab);
                go.transform.position = closestPoint;
                var controller = go.GetComponent<SpriteShapeController>();
                controller.spline.SetPosition(0,Vector3.zero);
                controller.spline.SetHeight(0,width);
                controller.spline.SetPosition(1,mouseWorldPosition - closestPoint);
                controller.spline.SetHeight(1,width);
                controller.GetComponent<SpriteShapeRenderer>().sortingOrder = closestController
                    ? closestController.GetComponent<SpriteShapeRenderer>().sortingOrder - 1
                    : -1;
                go.GetComponent<GameDraw>().init(closestPoint, mouseWorldPosition - closestPoint,width,closestController?closestController.GetComponent<GameDraw>():null);
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