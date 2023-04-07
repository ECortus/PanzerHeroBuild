using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class TankHeadController : MonoBehaviour
{
    public static TankHeadController Instance;

    bool _aiming = false;
    public bool Aiming
    {
        get 
        {
            return _aiming;
        }
        set
        {
            _aiming = value;
            PlayTypeUI.Instance.UpdateButtons();
        }
    }

    [SerializeField] private TankShooting tankShooting;
    [SerializeField] private TankShotButtonUI shotButton;

    [Space]
    [SerializeField] private GameObject aimUI;
    [SerializeField] private GameObject prepareAimCanvas;
    [SerializeField] private GameObject aimCanvas;

    [Space]
    [SerializeField] private Transform tank;
    [SerializeField] private Transform head;
    [SerializeField] private Transform gun, aimCamRoot;

    [Space]
    [SerializeField] private float limitAngleHorizontal;
    [SerializeField] private float minAngleVertical, maxAngleVertical;

    private Vector3 lastCamPos;
    private Quaternion rotation;
    private float mouseStartX, mouseStartY, diffMouseX, diffMouseY;

    [Space]
    [SerializeField] private float rotate;
    [SerializeField] private float sensivityVertical, sensivityHorizontal;

    bool rotating = false;

    public void On(float rotateY = 0f)
    {
        this.enabled = true;

        tank.localEulerAngles = new Vector3(0f, tank.localEulerAngles.y, 0f);

        if(rotateY != 0f)
        {
            StopAllCoroutines();
            StartCoroutine(RotateOnOn(rotateY));
        }
    }
    public void Off()
    {
        this.enabled = false;
    }

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator RotateOnOn(float y)
    {
        Quaternion headRot = Quaternion.Euler(0f, y, 0f);

        while(true)
        {
            head.localRotation = Quaternion.Slerp(head.localRotation, headRot, rotate / 3f * Time.deltaTime);

            yield return null;

            if(Input.GetMouseButtonDown(0))
            {
                break;
            }
        }

        yield return null;
    }

    void OnEnable()
    {
        aimUI.SetActive(true);
        prepareAimCanvas.SetActive(true);
        aimCanvas.SetActive(false);

        shotButton.Reset();

        gun.localEulerAngles = Vector3.zero;

        transform.parent.eulerAngles = new Vector3(0f, transform.parent.eulerAngles.y, 0f);

        rotating = false;
    }

    void OnDisable()
    {
        aimUI.SetActive(false);
    }

    void Update()
    {
        if(!PlayerStats.Instance.Active)
		{
			return;
		}

        if(TankShootPad.Instance.HaveTouch)
        {
            Down();

            rotating = true;
        }

        if(TankShootPad.Instance.IsPointerOverUIObject() && !Aiming && !rotating) return;

        if (Input.GetMouseButtonDown(0))
        {
            PrepareRotate();

            rotating = true;
        }

        if (Input.GetMouseButton(0) && rotating)
        {
            if(Aiming)
            {
                Rotate();
            }
            else
            {
                Rotate();
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(Aiming && !tankShooting.IsShooting && rotating) Shot();

            rotating = false;
        }
    }

    void Down()
    {
        PrepareRotate();

        if(Aiming || tankShooting.IsShooting) return;

        Aiming = true;

        prepareAimCanvas.SetActive(false);
        aimCanvas.SetActive(true);

        Transform root = aimCamRoot;
        GameManager.Instance.SetFollowTarget(root);

        TankShootPad.Instance.Off();
    }

    async void Shot()
    {
        await tankShooting.Shooting();
        Up();

        TankShootPad.Instance.On();
        Aiming = false;
    }

    public void Up()
    {
        prepareAimCanvas.SetActive(true);
        aimCanvas.SetActive(false);

        if(PlayerStats.Instance.Active)
        {
            Transform root = GameManager.Instance.prepareToAimCamRoot;
            GameManager.Instance.SetFollowTarget(root);
        }
        else
        {
            TankShootPad.Instance.On();
            Aiming = false;
        }
    }

    void PrepareRotate()
    {
        mouseStartX = Input.mousePosition.x / Screen.width;
        mouseStartY = Input.mousePosition.y / Screen.height;

        Vector3 last = new Vector3(
            0f,
            head.localRotation.eulerAngles.y,
            -gun.localRotation.eulerAngles.z
        );

        lastCamPos = last;
    }

    void Rotate()
    {
        diffMouseX = (mouseStartX - Input.mousePosition.x / Screen.width) * sensivityHorizontal;
        diffMouseY = (mouseStartY - Input.mousePosition.y / Screen.height) * sensivityVertical;

        rotation = Quaternion.Euler(0f, lastCamPos.y - diffMouseX, lastCamPos.z + diffMouseY);

        rotation = Clamping(rotation);

        Quaternion headRot = new Quaternion(0f, rotation.y, 0f, rotation.w);
        Quaternion gunRot = new Quaternion(0f, 0f, -rotation.z, rotation.w);

        head.localRotation = Quaternion.Slerp(head.localRotation, headRot, rotate * Time.deltaTime);
        gun.localRotation = Quaternion.Slerp(gun.localRotation, gunRot, rotate * Time.deltaTime);
    }

    Quaternion Clamping(Quaternion rot)
    {
        Vector3 angles = rot.eulerAngles;

        float Y = angles.y;
        float Z = angles.z;

        float minZ = minAngleVertical < 0f ? 360f + minAngleVertical : minAngleVertical;
        float maxZ = maxAngleVertical < 0f ? 360f + maxAngleVertical : maxAngleVertical;
        float minY = 360f - limitAngleHorizontal;
        float maxY = limitAngleHorizontal;

        Y = Clamp(Y, minY, maxY);
        Z = Clamp(Z, minZ, maxZ);

        angles = new Vector3(0f, Y, Z);

        return Quaternion.Euler(angles);
    }

    float Clamp(float angle, float min, float max)
    {
        float rtrn = 0f;

        if(angle < 180f)
        {
            if(angle < max && angle < min) rtrn = angle;
            else rtrn = max;
        }
        else if(angle > 180f)
        {
            if(angle > min && angle > max) rtrn = angle;
            else rtrn = min;
        }
        else
        {
            rtrn = 0f;
        }

        return rtrn;
    }
}
