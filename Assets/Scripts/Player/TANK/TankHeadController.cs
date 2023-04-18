using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class TankHeadController : MonoBehaviour
{
    public static TankHeadController Instance;

    public bool _aiming = false;
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

    public TankShooting tankShooting;
    [SerializeField] private TankShotButtonUI shotButton;

    [Space]
    [SerializeField] private GameObject aimUI;
    [SerializeField] private GameObject prepareAimCanvas;
    [SerializeField] private GameObject aimCanvas;

    [Space]
    [SerializeField] private Transform tank;
    [SerializeField] private Transform head;
    public Transform prepareToAimRoot;
    [SerializeField] private Transform gunPivot, gun, aimCamRoot;

    [Space]
    [SerializeField] private float limitAngleHorizontal;
    [SerializeField] private float minAngleVertical, maxAngleVertical;

    private Vector3 lastCamPos;
    private Quaternion rotation;
    private float mouseStartX, mouseStartY, diffMouseX, diffMouseY;

    [Space]
    [SerializeField] private float rotate;
    [SerializeField] private float sensivityVertical, sensivityHorizontal;

    [Space]
    [SerializeField] private float boundForShot;

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

        gunPivot.localEulerAngles = Vector3.zero;

        transform.parent.eulerAngles = new Vector3(0f, transform.parent.eulerAngles.y, 0f);

        rotating = false;

        if(Tutorial.Instance != null)
        {
            if(!Tutorial.Instance.Complete && !Tutorial.Instance.ROTATE_isDone)
            {
                Tutorial.Instance.SetState(TutorialState.ROTATE, true);
            }
            else if(!Tutorial.Instance.ROTATE_isDone)
            {
                Tutorial.Instance.SHOOT_isDone = false;
                Tutorial.Instance.SetState(TutorialState.ROTATE, true);
            }
        }
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

        /* if (Input.GetMouseButtonDown(0))
        {
            PrepareRotate();

            rotating = true;
        } */

        if (Input.GetMouseButton(0) && rotating)
        {
            Rotate();
        }

        if(Input.GetMouseButtonUp(0))
        {
            /* if(Aiming && !tankShooting.IsShooting && rotating) Shot(); */

            if(new Vector2(diffMouseX, diffMouseY).magnitude < boundForShot &&
                /* Aiming && */!tankShooting.IsShooting && rotating)
            {
                Shot();
            }
            else
            {
                Up();
            }
            
            rotating = false;
        }
    }

    void Down()
    {
        PrepareRotate();

        if(Aiming/*  || tankShooting.IsShooting */) return;

        /* Aiming = true; */

        /* prepareAimCanvas.SetActive(false);
        aimCanvas.SetActive(true);

        Transform root = aimCamRoot;
        GameManager.Instance.SetFollowTarget(root); */

        TankShootPad.Instance.Off();
    }

    void CorrectGunDirection()
    {
        /* RaycastHit hit;
        if(Physics.Raycast(prepareToAimRoot.position, prepareToAimRoot.forward, out hit))
        {
            float angle = 0f;

            Vector3 pos = new Vector3(prepareToAimRoot.position.x, 0f, 0f);
            Vector3 point = hit.point;
            point.y = 0f;
            point.z = 0f;

            float catet1 = Vector3.Distance(point, pos);
            float catet2 = prepareToAimRoot.localPosition.y;

            angle = Mathf.Tan(catet2/catet1);

            gun.localEulerAngles = new Vector3(0f, 0f, angle * 1.5f);
        }
        else
        {
            gun.localEulerAngles = new Vector3(0f, 0f, 1f);
        } */
        gun.localEulerAngles = new Vector3(0f, 0f, 1.2f);
    }

    async void Shot()
    {
        Aiming = true;

        CorrectGunDirection();

        await tankShooting.Shooting();
        Up();

        if(Tutorial.Instance != null)
        {
            if(!Tutorial.Instance.Complete)
            {
                if(Tutorial.Instance.SHOOT_isDone && !Tutorial.Instance.RIDE_isDone) Tutorial.Instance.SetState(TutorialState.NONE);
            }
            else
            {
                if(Tutorial.Instance.SHOOT_isDone) Tutorial.Instance.SetState(TutorialState.NONE);
            }
        }

        TankShootPad.Instance.On();
        Aiming = false;
    }

    public void Up()
    {
        /* prepareAimCanvas.SetActive(true);
        aimCanvas.SetActive(false);

        Transform root = GameManager.Instance.prepareToAimCamRoot;
        GameManager.Instance.SetFollowTarget(root); */

        if(Tutorial.Instance != null)
        {
            if(!Tutorial.Instance.SHOOT_isDone) Tutorial.Instance.SetState(TutorialState.SHOOT, true);
            /* if(!Tutorial.Instance.Complete)
            {
                if(!Tutorial.Instance.SHOOT_isDone) Tutorial.Instance.SetState(TutorialState.SHOOT, true);
            } */
        }
        
        TankShootPad.Instance.On();
        Aiming = false;
    }

    void PrepareRotate()
    {
        mouseStartX = Input.mousePosition.x / Screen.width;
        mouseStartY = Input.mousePosition.y / Screen.height;

        Vector3 last = new Vector3(
            0f,
            head.localRotation.eulerAngles.y,
            -gunPivot.localRotation.eulerAngles.z
        );

        lastCamPos = last;

        diffMouseX = 0f;
        diffMouseY = 0f;
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
        gunPivot.localRotation = Quaternion.Slerp(gunPivot.localRotation, gunRot, rotate * Time.deltaTime);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if(GameManager.Instance != null) 
            Gizmos.DrawRay(prepareToAimRoot.position, prepareToAimRoot.forward * 200f);
    }
}
