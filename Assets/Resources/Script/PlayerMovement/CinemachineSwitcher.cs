using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    public CinemachineFreeLook freeLookCam;
    public bool usingFreeLook = false;

    private Coroutine fovCoroutine;

    void Start()
    {
        virtualCam.Priority = 20;
        freeLookCam.Priority = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (usingFreeLook)
            {
                virtualCam.Priority = 20;
                freeLookCam.Priority = 0;
            }
            else
            {
                virtualCam.Priority = 0;
                freeLookCam.Priority = 20;
            }
            usingFreeLook = !usingFreeLook;
        }

        if (Input.GetKey(KeyCode.E))
        {
            SetFov(100, 1);
        }
        
    }

    public void SetFov(float fov, float duration)
    {
        if (fovCoroutine != null)
        {
            StopCoroutine(fovCoroutine);
        }

        fovCoroutine = StartCoroutine(SmoothSetFovCoroutine(fov, duration));
    }

    private IEnumerator SmoothSetFovCoroutine(float targetFov, float duration)
    {
        float elapsedTime = 0f;
        float startFov = virtualCam.m_Lens.FieldOfView;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(startFov, targetFov, t);
            freeLookCam.m_Lens.FieldOfView = Mathf.Lerp(startFov, targetFov, t);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        virtualCam.m_Lens.FieldOfView = targetFov;
        freeLookCam.m_Lens.FieldOfView = targetFov;
    }
}
