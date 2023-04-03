using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateMeshes : MonoBehaviour
{
    [SerializeField] private GameObject[] unharmedMeshes = new GameObject[3];
    [SerializeField] private GameObject[] harmedMeshes = new GameObject[3];

    public void Upd(bool broken)
    {
        for(int i = 0; i < unharmedMeshes.Length; i++)
        {
            unharmedMeshes[i].SetActive(!broken);
        }

        for(int i = 0; i < harmedMeshes.Length; i++)
        {
            harmedMeshes[i].SetActive(broken);
        }
    }
}
