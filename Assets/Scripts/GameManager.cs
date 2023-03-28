using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    private bool _GameActive = false;
    public void SetActive(bool value) => _GameActive = value;
    public bool GetActive() => _GameActive;

    [Header("Camera: ")]
    public PlayerCamera Camera;
    public Transform rideCamRoot;
    public Transform prepareToAimCamRoot;

    void Awake() => Instance = this;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Money.Plus(500);
        }
    }

    public void SetFollowTarget(Transform tf)
    {
        Camera.SetFollow(tf);
    }
}
