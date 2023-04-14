using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
public class TankShooting : MonoBehaviour
{
    [HideInInspector] public bool IsShooting = false;

    [SerializeField] private TankHeadController headController;

    [Space]
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform inertiaPivot;
    [SerializeField] private GameObject whizzbangPrefab;

    [Space]
    [SerializeField] private float inertiaForce;

    [Space]
    [SerializeField] private float ReloadDelta = 5f/6f;
    [HideInInspector] public bool isReloading = false;
    private int ReloadTime => (int)(PlayerStats.Instance.TimeReload * 1000);

    public async UniTask Shooting()
    {
        IsShooting = true;

        await Shoot();

        /* while(true)
        {
            if(!PlayerStats.Instance.Active) break;

            if(await Shoot()) break;

            if(Input.GetMouseButton(0))
            {
                await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0));
            }
            else
            {
                break;
            }
        } */

        isReloading = false;
        IsShooting = false;
    }

    public async UniTask<bool> Shoot()
    {
        Vector3 pos = muzzle.position;
        Vector3 rot = muzzle.eulerAngles;
        GameObject go = ObjectPool.Instance.Insert(ObjectType.Whizzbang, whizzbangPrefab, pos, rot);
        go.GetComponent<Whizzbang>().damage = PlayerStats.Instance.Damage;

        return await UseWhizzbang();
    }

    public async UniTask<bool> UseWhizzbang()
    {
        isReloading = false;

        PlayerStats.Instance.WhizzbangCount -= 1;

        Inertia();
        
        if(PlayerStats.Instance.WhizzbangCount == 0)
        {
            isReloading = true;

            headController.Up();
            await Reload();
        }
        else
        {
            /* await UniTask.Delay(ReloadTime); */
            await UniTask.Delay(10);
        }

        return isReloading;
    }

    async UniTask Reload()
    {
        await UniTask.Delay(50);
        await UI.Instance.Reload((int)(ReloadTime * ReloadDelta));
    }

    float duration;
    float time = 0f;
    float originalZ;

    void Inertia()
    {
        duration = (ReloadTime / 1000f) / 1.5f;

        MoveBack();
        CameraShake.Instance.On(duration);
    }

    void MoveBack()
    {
        StopAllCoroutines();
        StartCoroutine(Backward());
    }

    IEnumerator Backward()
    {
        time = duration;

        while(time > 0f)
        {
            inertiaPivot.localPosition = new Vector3(
                inertiaPivot.localPosition.x,
                inertiaPivot.localPosition.y,
                originalZ - (time / duration) * inertiaForce
            );
			time -= Time.deltaTime;

            yield return null;
        }

        time = 0f;
		inertiaPivot.localPosition = new Vector3(
            inertiaPivot.localPosition.x,
            inertiaPivot.localPosition.y,
            originalZ
        );

        yield return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(muzzle.position, muzzle.forward * 200f);
    }
}
