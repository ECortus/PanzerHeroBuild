using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerate : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    [SerializeField] private Vector2 size = new Vector2();
    [SerializeField] private float offset = 50f;

    [ContextMenu("Generate")]
    void Generate()
    {
        int count = transform.childCount;
        for(int i = 0; i < count; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                Vector3 pos = new Vector3(
                    offset * x,
                    0f,
                    offset * y
                );

                GameObject go = Instantiate(plane, transform);
                go.transform.localPosition = pos;
            }
        }
    }
}
