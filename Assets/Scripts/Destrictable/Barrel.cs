using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Barrel : MonoBehaviour
{
    [SerializeField] private LayerMask hitMasks; /// destrictable, unit, tank
    public float radius;

    [Space]
    [SerializeField] private GameObject effect;

    [Space]
    [SerializeField] private Collider collid;
    [SerializeField] private GameObject barrel;

    public Vector3 center
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
            case "Bullet":
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
            case "Destrictable":
                BOOM();
                break;
            default:
                break;
        }
    }

    async void BOOM()
    {  
        Collider[] cols = Physics.OverlapSphere(center, radius, hitMasks);

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
            else if(destrictable == null || !buildings.Contains(destrictable.building))
            {
                if(col.TryGetComponent<DestrictableObject>(out destrictable))
                {
                    destrictable.building.GetDestroyedByBarrel(this);
                    buildings.Add(destrictable.building);
                    continue;
                }
            }
            else if(Vector3.Distance(TankController.Instance.Transform.position, center) <= radius)
            {
                PlayerStats.Instance.GetHit(5f);
                continue;
            }
        }

        barrel.SetActive(false);

        if(effect != null) 
            ParticlePool.Instance.Insert(ParticleType.BarrelBoomEffect, effect, center);

        collid.enabled = false;

        await UniTask.Delay(2000);
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, radius);
    }
}
