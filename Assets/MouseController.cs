// using UnityEngine;
// using UnityEngine.U2D;
//
// public class MouseController : MonoBehaviour
// {
//     public SpriteShapeController spriteShapeController;
//     public int numberOfSamples = 100;
//     public Transform marker;
//
//     private void Update()
//     {
//         Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         mouseWorldPosition.z = 0;
//
//         float closestDistance = Mathf.Infinity;
//         Vector3 closestPoint = Vector3.zero;
//
//         for (int i = 0; i < numberOfSamples; i++)
//         {
//             float t = (float)i / numberOfSamples;
//             Vector3 point = spriteShapeController.spline.GetPosition(SplineUtility.Interpolate(spriteShapeController.spline.GetPointCount(), t));
//             float distance = Vector3.Distance(mouseWorldPosition, point);
//
//             if (distance < closestDistance)
//             {
//                 closestDistance = distance;
//                 closestPoint = point;
//             }
//         }
//
//         if (marker != null)
//         {
//             marker.position = closestPoint;
//         }
//
//         Debug.Log("Closest point on SpriteShape: " + closestPoint);
//     }
// }