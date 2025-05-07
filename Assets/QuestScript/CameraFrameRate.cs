using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFrameRate : MonoBehaviour
{
    public float FPS = 5f;
    public Camera renderCam;
    public bool OnCam;
    void Start()
    {
        renderCam = GetComponent<Camera>();
        renderCam.enabled = false;
        StartCoroutine(DelayedRendering());
    }

    public IEnumerator DelayedRendering()
    {
        while (true)
        {
            if (OnCam)
            {
                renderCam.Render();
            }

            yield return new WaitForSeconds(1 / FPS);

        }
    }
}
