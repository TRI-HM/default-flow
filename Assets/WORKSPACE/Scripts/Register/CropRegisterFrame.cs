using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropRegisterFrame : MonoBehaviour
{
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            rectTransform.sizeDelta = new Vector2(screenWidth * 0.8f, screenHeight * 0.7f);
        }
    }

}
