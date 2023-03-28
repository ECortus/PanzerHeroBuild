using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
public class TankShooting : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [HideInInspector] public bool IsShooting = false;

    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject whizzbangPrefab;

    private int ReloadTime => (int)(500f/*  / Modifications.TimeReloadMod */);

    public async UniTask Shooting()
    {
        IsShooting = true;

        while(true)
        {
            await Shoot();
            if(Input.GetMouseButton(0))
            {
                await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0));
            }
            else
            {
                break;
            }
        }

        IsShooting = false;
    }

    public async UniTask Shoot()
    {
        Vector3 pos = muzzle.position;
        Vector3 rot = muzzle.eulerAngles;
        GameObject go = ObjectPool.Instance.Insert(ObjectType.Whizzbang, whizzbangPrefab, pos, rot);
        go.GetComponent<Whizzbang>().damage = stats.Damage;

        await UseWhizzbang();
    }

    public async UniTask UseWhizzbang()
    {
        PlayerStats.Instance.WhizzbangCount -= 1;
        
        if(PlayerStats.Instance.WhizzbangCount == 0)
        {
            await Reload();
        }
        
        await UniTask.Delay(ReloadTime);
    }

    async UniTask Reload()
    {
        for(int i = 0; i < PlayerStats.Instance.MaxWhizzbangCount; i++)
        {
            await UniTask.Delay(ReloadTime);
            PlayerStats.Instance.WhizzbangCount += 1;
        }
    }
}
