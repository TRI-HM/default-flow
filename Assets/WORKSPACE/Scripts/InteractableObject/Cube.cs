using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        // Thực hiện hành động khi đối tượng được tương tác
        Debug.LogWarning("Cube đã được tương tác!");
    }
}
