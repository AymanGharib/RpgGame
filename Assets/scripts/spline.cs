using UnityEngine;

public class SplineInterpolator : MonoBehaviour
{

    public GameObject halway;
    public Vector3[] CreateSpline(Vector3 startPoint, Vector3 endPoint, int numPoints)
    {
        Vector3[] splinePoints = new Vector3[numPoints];

        // Calculate control points based on start and end points
        Vector3 startTangent = startPoint + (endPoint - startPoint) / 3f;
        Vector3 endTangent = endPoint - (endPoint - startPoint) / 3f;

        // Calculate spline points
        for (int i = 0; i < numPoints; i++)
        {
            float t = (float)i / (numPoints - 1);
            splinePoints[i] = CalculateCubicBezierPoint(startPoint, startTangent, endTangent, endPoint, t);
        }

        return splinePoints;
    }

    // Function to calculate a point on a cubic Bezier curve
    private Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0; // (1-t)^3 * P0
        point += 3f * uu * t * p1; // 3*(1-t)^2 * t * P1
        point += 3f * u * tt * p2; // 3*(1-t) * t^2 * P2
        point += ttt * p3; // t^3 * P3

        return point;
    }

    // Example usage
    void Start()
    {
        Vector3 startPoint = new Vector3(-10, 0, 0);
        Vector3 endPoint = new Vector3(0, 5, 0);
        int numPoints = 10;

        Vector3[] splinePoints = CreateSpline(startPoint, endPoint, numPoints);
        for (int i = 0; i < splinePoints.Length - 1; i++)
        {
            Debug.DrawLine(splinePoints[i], splinePoints[i + 1], Color.red, 500f);
        }   // Do something with the spline points, such as drawing a line or placing objects along the path
    }
    }

