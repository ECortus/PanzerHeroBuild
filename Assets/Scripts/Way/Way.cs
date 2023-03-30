using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class Way : MonoBehaviour
{
    [SerializeField] private bool UpdateAlready = false;

    [Header("Points && dots: ")]
    public List<Vector3> Points = new List<Vector3>();

    [Space]
    [SerializeField] private Transform startDot;
    [SerializeField] private Transform finishDot;

    [SerializeField] private Transform dotsTransform;
    private List<Transform> _dots = new List<Transform>();
    private List<Transform> dots
    {
        get
        {
            if(_dots.Count == 0 || _dots.Count != dotsTransform.childCount + 2) _dots = FormList();
            return _dots;
        }
    }

    [Header("Smoothing: ")]
    [SerializeField] private bool ToSmooth = false;
    [Range(0, 35)]
    [SerializeField] private int smoothSections = 10;
    [Range(0f, 15f)]
    /* [SerializeField] private float smoothDistance = 5f;
    private BezierCurve[] Curves; */

    [Space]
    [SerializeField] private LineRenderer line;

    void Start()
    {
        UpdateAlready = false;
        DrawLine();
    }

    List<Transform> FormList()  ///first - fake, second - real
    {
        List<Transform> list = new List<Transform>();
        if(startDot != null) list.Add(startDot);

        List<Transform> dotsList = dotsTransform.GetComponentsInChildren<Transform>().ToList();
        dotsList.RemoveAt(0);
        list.AddRange(dotsList);

        if(finishDot != null) list.Add(finishDot);

        return list;
    }

    void Update()
    {
        if(UpdateAlready) DrawLine();   
    }

    void DrawLine()
    {
        if(line == null || dotsTransform == null)
        {
            line.positionCount = 0;
            Points.Clear();
            return;
        }

        DrawDefaultLine();

        if(ToSmooth && dots.Count > 2)
        {
            DrawSmoothLine();
        }
    }

    void DrawDefaultLine()
    {
        Points.Clear();
        line.positionCount = dots.Count;

        for(int i = 0; i < dots.Count; i++)
        {
            Vector3 point = dots[i].position;
            Points.Add(point);
            line.SetPosition(i, point);
        }
    }

    void DrawSmoothLine()
    {
        /* MatchCurvesToDots(); */
        List<Vector3> smoothedPoints = new List<Vector3>();
        smoothedPoints = MakeSmoothCurve(Points.ToArray(), smoothSections);
        
        /* for (int i = 0; i < Curves.Length; i++)
        {
            Vector3[] segments = Curves[i].GetSegments(smoothSections);
            for (int j = 0; j < segments.Length; j++)
            {
                smoothedPoints.Add(segments[j]);
            }
        } */

        Points.Clear();
        /* line.positionCount = Curves.Length * smoothSections - (smoothSections / 5); */
        line.positionCount = smoothedPoints.Count;

        for(int i = 0; i < line.positionCount; i++)
        {
            Vector3 point = smoothedPoints[i];
            Points.Add(point);
            line.SetPosition(i, point);
        }
    }

    /* private void MatchCurvesToDots()
    {
        EnsureCurvesMatchLineRendererPositions();

        for (int i = 0; i < Curves.Length; i++)
        {
            Vector3 position = line.GetPosition(i);
            Vector3 lastPosition = i == 0 ? line.GetPosition(0) : line.GetPosition(i - 1);
            Vector3 nextPosition = line.GetPosition(i + 1);

            Vector3 lastDirection = (position - lastPosition).normalized;
            Vector3 nextDirection = (nextPosition - position).normalized;

            Vector3 startTangent = (lastDirection + nextDirection) * smoothDistance;
            Vector3 endTangent = (nextDirection + lastDirection) * -1 * smoothDistance;

            Curves[i].Points[0] = position; // Start Position (P0)
            Curves[i].Points[1] = position + startTangent; // Start Tangent (P1)
            Curves[i].Points[2] = nextPosition + endTangent; // End Tangent (P2)
            Curves[i].Points[3] = nextPosition; // End Position (P3)
        }

        Vector3 nextDir = (Curves[1].EndPosition - Curves[1].StartPosition).normalized;
        Vector3 lastDir = (Curves[0].EndPosition - Curves[0].StartPosition).normalized;

        Curves[0].Points[2] = Curves[0].Points[3] + (nextDir + lastDir) * -1 * smoothDistance;
    }

    private void EnsureCurvesMatchLineRendererPositions()
    {
        if (Curves == null || Curves.Length != line.positionCount - 1)
        {
            Curves = new BezierCurve[line.positionCount - 1];
            for (int i = 0; i < Curves.Length; i++)
            {
                Curves[i] = new BezierCurve();
            }
        }
    } */

    private List<Vector3> MakeSmoothCurve(Vector3[] arrayToCurve, float smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints);
    }
}
