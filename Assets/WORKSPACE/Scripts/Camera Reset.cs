using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Android;
using UnityEngine.XR.Management;
using System.Collections;

public class CameraReset : MonoBehaviour
{
    void Start()
    {
        // Kích hoạt lại XR khi chuyển scene
        if (Camera.main != null)
        {
            Camera.main.gameObject.SetActive(true);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        StartCoroutine(RestartXR());
    }

    IEnumerator RestartXR()
    {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        yield return null;
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }
}
