using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frameRate : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    float deltaTime;

    private IEnumerator Start()
    {
        while(true)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            text.text = Mathf.Ceil(fps).ToString();
            yield return new WaitForSeconds(0.1488f);
        }
    }
}
