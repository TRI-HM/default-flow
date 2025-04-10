using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IInteractable
{
    public GameObject ObjPopupInfo;
    private bool isPopupActive = false;
    void Start()
    {
        if (ObjPopupInfo != null)
        {
            ObjPopupInfo.SetActive(false);
        }
    }
    public void OnInteract()
    {
        if (isPopupActive)
        {
            ObjPopupInfo.SetActive(false);
            isPopupActive = false;
            return;
        }
        isPopupActive = true;
        ObjPopupInfo.SetActive(true);
    }
}
