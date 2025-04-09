using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IInteractable
{
    public GameObject ObjPopupInfo;

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
    }
}
