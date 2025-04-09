using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClick : MonoBehaviour, IPointerClickHandler
{
    // Phương thức này sẽ được gọi khi vật thể bị chạm (clicked)
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogWarning("Đã chạm vào vật thể!");
    }
}
