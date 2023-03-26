using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhizzbangCollision : MonoBehaviour
{
    [SerializeField] private Whizzbang main;

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        switch(go.tag)
        {
            case "Untagged":
                main.HitAboveSomething();
                break;
            case "Building":
                main.HitAboveSomething();
                break;
            case "Destrictable":
                DestrictableBuilding building = col.gameObject.GetComponent<DestrictableObject>().building;
                building.GetDestroyedByWhizzbang(main);
                main.HitAboveSomething();
                break;
            case "Ground":
                main.HitAboveSomething();
                break;
            default:
                break;
        }

        Debug.Log(go.tag);
    }
}
