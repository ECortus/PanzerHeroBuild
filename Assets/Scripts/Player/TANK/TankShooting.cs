using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
public class TankShooting : MonoBehaviour
{
    [HideInInspector] public bool IsShooting = false;

    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject whizzbangPrefab;

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
        ObjectPool.Instance.Insert(ObjectType.Whizzbang, whizzbangPrefab, pos, rot);

        await UseWhizzbang();
    }

    public async UniTask UseWhizzbang()
    {
        PlayerStats.Instance.WhizzbangCount -= 1;
        
        if(PlayerStats.Instance.WhizzbangCount == 0)
        {
            await Reload();
        }
        
        await UniTask.Delay(400);
    }

    async UniTask Reload()
    {
        for(int i = 0; i < PlayerStats.Instance.MaxWhizzbangCount; i++)
        {
            await UniTask.Delay(500);
            PlayerStats.Instance.WhizzbangCount += 1;
        }
    }
}
