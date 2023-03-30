using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] private Bullet main;

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        main.rb.velocity = Vector3.zero;
        switch(go.tag)
        {
            case "Untagged":
                main.HitAboveSomething();
                break;
            case "Player":
                PlayerStats.Instance.GetHit(main.damage);
                main.HitAboveSomething();
                break;
            case "Ground":
                main.HitAboveSomething();
                break;
            default:
                break;
        }

        Debug.Log($"{gameObject.name} collision: {go.tag}");
    }
}
