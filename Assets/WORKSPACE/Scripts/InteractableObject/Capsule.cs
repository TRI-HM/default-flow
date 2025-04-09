using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        // Thực hiện hành động khi đối tượng được tương tác
        Debug.LogWarning("Capsule đã được tương tác!");
    }
}
