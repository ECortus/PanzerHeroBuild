using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterNoise : MonoBehaviour
{
    float power = 1f;
    float scale = 1f;
    float timeScale = 1f;

    float offsetX;
    float offsetY;
    [SerializeField] MeshFilter _meshFilter;
    WaterParam _param;

    [HideInInspector] public float length = 0f;
    public Transform WaveOrigin;

    void Start()
    {
        _param = GetComponentInParent<WaterParam>();
        power = _param.powerScale;
        scale = _param.scale;
        timeScale = _param.timeScale;

        _meshFilter = GetComponent<MeshFilter>();
        /* MakeNoise(); */
    }

    void Update()
    {
        if(length > 0f)
        {
            power = _param.powerScale;
            scale = _param.scale;
            timeScale = _param.timeScale;

            MakeNoise();
            offsetX += Time.deltaTime * timeScale;
            offsetY += Time.deltaTime * timeScale;
        }
    }

    void MakeNoise()
    {
        Vector3[] verticies = _meshFilter.mesh.vertices;

        Vector3 point = WaveOrigin.localPosition;
        float distance = 0f;
        float delta = 0f;

        for(int i = 0; i < verticies.Length; i++)
        {
            distance = Vector3.Distance(point, verticies[i]);
            delta = (1f - (distance / length));

            if(delta < 0f) continue;

            verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z) * power * delta;
        }

        _meshFilter.mesh.vertices = verticies;
    } 

    float CalculateHeight(float x, float y)
    {
        float coordX = x * scale + offsetX;
        float coordY = y * scale + offsetY;

        return Mathf.PerlinNoise(coordX, coordY);
    }
}
