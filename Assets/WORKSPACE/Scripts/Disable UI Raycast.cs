using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableUIRaycast : MonoBehaviour
{
    void Start()
    {
        Graphic g = GetComponent<Graphic>();
        if (g != null)
        {
            g.raycastTarget = false;
        }

    }
}