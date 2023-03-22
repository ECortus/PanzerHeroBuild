using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class Way : MonoBehaviour
{
    [SerializeField] private bool UpdateAlready = false;

    public List<Vector3> Points = new List<Vector3>();

    [SerializeField] private Transform dotsTransform;
    private List<Transform> dots
    {
        get
        {
            List<Transform> list = dotsTransform.GetComponentsInChildren<Transform>().ToList();
            list.RemoveAt(0);
            return list;
        }
    }
    [SerializeField] private LineRenderer line;

    void Start()
    {
        DrawLine();
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

        Points.Clear();
        List<Transform> dotsList = dots;
        line.positionCount = dotsList.Count;

        for(int i = 0; i < dotsList.Count; i++)
        {
            Vector3 point = dotsList[i].position;
            line.SetPosition(i, point);
            Points.Add(point);
        }
    }
}
