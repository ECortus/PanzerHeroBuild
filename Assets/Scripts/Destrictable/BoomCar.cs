using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class BoomCar : MonoBehaviour, IBoom
{
    [SerializeField] private LayerMask hitMasks; /// destrictable, unit, tank
    [SerializeField] private float _radius;
    public float Radius { get { return _radius; } }

    [Space]
    [SerializeField] private GameObject effect;

    [SerializeField] private UnityEvent boom;

    public Vector3 Center
	{
		get
		{
			return transform.position + new Vector3(0f, 3f, 0f);
		}
	}

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        switch(go.tag)
        {
            case "Player":
                BOOM();
                break;
            case "Whizzbang":
                BOOM();
                break;
            case "EnemyJeep":
                BOOM();
                break;
            case "EnemyTank":
                BOOM();
                break;
            default:
                break;
        }
    }

    void BOOM()
    {  
        Collider[] cols = Physics.OverlapSphere(Center, Radius, hitMasks);

        EnemyStats enemy = null;
        DestrictableObject destrictable = null;

        List<DestrictableBuilding> buildings = new List<DestrictableBuilding>();

        foreach(Collider col in cols)
        {
            if(col.TryGetComponent<EnemyStats>(out enemy))
            {
                enemy.GetHit(999f);
                continue;
            }
            else if(col.TryGetComponent<DestrictableObject>(out destrictable))
            {
                if(destrictable != null && !buildings.Contains(destrictable.building))
                {
                    /* float distance = (destrictable.transform.position - transform.position).magnitude; */
                    destrictable.building.GetDestroyedByBoom(this/* , distance */);
                    buildings.Add(destrictable.building);
                    continue;
                }
            }
            /* else if(Vector3.Distance(TankController.Instance.Transform.position, Center) <= Radius)
            {
                PlayerStats.Instance.GetHit(5f);
                continue;
            } */
        }

        if(effect != null) 
            ParticlePool.Instance.Insert(ParticleType.BarrelBoomEffect, effect, Center);

        GetBoomed();
    }

    public void ForceBodyUp(Rigidbody bodyRB)
    {
        Rigidbody rigid = bodyRB;
        rigid.useGravity = true;

        rigid.AddForce(100000f * transform.up);
        rigid.angularVelocity = new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f)
        );
    }

    void GetBoomed()
    {
        boom.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Center, Radius);
    }
}
