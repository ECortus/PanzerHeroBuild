using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; set; }
	public Transform cameraTransform;
	
	public float shakeForce = 0.7f;
    private float duration = 0f;
	
	Vector2 originalPos;

    void Awake() => Instance = this;

    public void On(float dur)
    {
        duration = dur;
        originalPos = cameraTransform.localPosition;

        StopAllCoroutines();
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        while(duration > 0f)
        {
            Vector3 random = Random.insideUnitSphere;
            random.z = 0f;
            cameraTransform.localPosition = new Vector3(originalPos.x, originalPos.y, cameraTransform.localPosition.z) + random * shakeForce;
			duration -= Time.deltaTime;

            yield return null;
        }

        duration = 0f;
		cameraTransform.localPosition = new Vector3(originalPos.x, originalPos.y, cameraTransform.localPosition.z);

        yield return null;
    }
}
