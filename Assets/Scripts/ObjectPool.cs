using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    void Awake() => Instance = this;

    private List<Whizzbang> WhizzbangPool = new List<Whizzbang>();
    private List<Bullet> BulletPool = new List<Bullet>();

    public GameObject Insert(ObjectType type, GameObject obj, Vector3 pos, Vector3 rot)
    {
        if(type == ObjectType.Whizzbang)
        {
            foreach(Whizzbang wb in WhizzbangPool)
            {
                if(wb == null) continue;

                if(!wb.Active) 
                {
                    wb.Reset(pos, rot);
                    wb.On();
                    return wb.gameObject;
                }
            }

            Whizzbang scr = Instantiate(obj, pos, Quaternion.Euler(rot)).GetComponent<Whizzbang>();
            WhizzbangPool.Add(scr);
            return scr.gameObject;
        }

        if(type == ObjectType.Bullet)
        {
            foreach(Bullet bul in BulletPool)
            {
                if(bul == null) continue;

                if(!bul.Active) 
                {
                    bul.Reset(pos, rot);
                    bul.On();
                    return bul.gameObject;
                }
            }

            Bullet scr = Instantiate(obj, pos, Quaternion.Euler(rot)).GetComponent<Bullet>();
            BulletPool.Add(scr);
            return scr.gameObject;
        }

        return null;
    }
}

[System.Serializable]
public enum ObjectType
{
    Default, Whizzbang, Bullet
}
