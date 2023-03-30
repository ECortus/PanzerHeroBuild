using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HPBarObject: MonoBehaviour
{
    public GameObject GameObject => gameObject;
    public Slider HPFill;
    public Slider ReduceFill;

    public void On() => gameObject.SetActive(true);
    public void Off() => gameObject.SetActive(false);
}
