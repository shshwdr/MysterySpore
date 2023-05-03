using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserLine : MonoBehaviour
{
    public Transform startPoint;
    public Vector3 endPoint;
    public Color startColor = Color.red;
    public Color endColor = Color.red;
    public float lineWidth = 0.1f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (startPoint)
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint);
        }
    }
}