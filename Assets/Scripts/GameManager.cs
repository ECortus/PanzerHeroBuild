using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [Header("Camera: ")]
    public PlayerCamera Camera;

    void Awake() => Instance = this;

    void Start()
    {
        Application.targetFrameRate = 60;
        DataManager.Load();
    }

    public void SetFollowTarget(Transform tf)
    {
        Camera.SetFollow(tf);
    }
}
