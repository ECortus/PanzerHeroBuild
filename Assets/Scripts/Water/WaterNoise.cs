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

    void Start()
    {
        _param = GetComponentInParent<WaterParam>();
        power = _param.powerScale;
        scale = _param.scale;
        timeScale = _param.timeScale;

        _meshFilter = GetComponent<MeshFilter>();
        MakeNoise();
    }

    void Update()
    {
        power = _param.powerScale;
        scale = _param.scale;
        timeScale = _param.timeScale;

        MakeNoise();
        offsetX += Time.deltaTime * timeScale;
        offsetY += Time.deltaTime * timeScale;
    }

    void MakeNoise()
    {
        Vector3[] verticies = _meshFilter.mesh.vertices;

        for(int i = 0; i < verticies.Length; i++)
        {
            verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z) * power;
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
