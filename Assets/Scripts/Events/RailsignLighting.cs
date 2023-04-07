using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class RailsignLighting : MonoBehaviour
{
    private int materialIndex = 3;
    private float sparkleDelay = 0.5f;

    [Space]
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Material red, yellow, off;
    private float time;

    void Start()
    {
        mesh.materials[materialIndex] = off;
        On(999);
    }

    public void On(float value)
    {
        /* time = 0f;

        while(time < value)
        {
            mesh.materials[materialIndex] = red;
            await UniTask.Delay(sparkleDelay / 2);

            mesh.materials[materialIndex] = yellow;
            await UniTask.Delay(sparkleDelay / 2);

            time += sparkleDelay;
        }

        mesh.materials[materialIndex] = off; */
        StopAllCoroutines();
        StartCoroutine(Lighting(value));
    }

    IEnumerator Lighting(float duration)
    {
        yield return null;

        WaitForSeconds wait = new WaitForSeconds(sparkleDelay / 2);
        time = 0f;

        while(time < duration)
        {
            ChagneMaterial(materialIndex, red);
            yield return wait;

            ChagneMaterial(materialIndex, yellow);
            yield return wait;

            time += sparkleDelay;
        }

        ChagneMaterial(materialIndex, off);
    }

    void ChagneMaterial(int index, Material material)
    {
        Material[] materials = mesh.materials;
        materials[index] = material;
        mesh.materials = materials;
    }
}
