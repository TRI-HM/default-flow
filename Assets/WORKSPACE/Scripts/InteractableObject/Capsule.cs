using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour, IInteractable
{
    public GameObject ObjPopupInfo;
    private Coroutine deactivationCoroutine;
    private float deactivationDelay = 5f;
    void Start()
    {
        if (ObjPopupInfo != null)
        {
            ObjPopupInfo.SetActive(false);
        }
    }
    public void OnInteract()
    {
        ObjPopupInfo.SetActive(true);

        if (deactivationCoroutine != null)
        {
            StopCoroutine(deactivationCoroutine);
        }
        deactivationCoroutine = StartCoroutine(DeactivatePopupAfterDelay(deactivationDelay));
    }

    private IEnumerator DeactivatePopupAfterDelay(float delay)
    {
        // Đợi trong khoảng thời gian được chỉ định
        yield return new WaitForSeconds(delay);

        // Tắt đối tượng popup
        if (ObjPopupInfo != null)
        {
            ObjPopupInfo.SetActive(false);
        }

        // Reset biến coroutine
        deactivationCoroutine = null;
    }
    private void OnDestroy()
    {
        if (deactivationCoroutine != null)
        {
            StopCoroutine(deactivationCoroutine);
        }
    }
}
