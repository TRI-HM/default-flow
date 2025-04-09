using UnityEngine;
using System.Collections.Generic;

public class ControlButtonDesk : MonoBehaviour
{
    // Danh sách các nút 3D cần quản lý, bạn gán từ Inspector
    public List<GameObject> buttons;

    void Start()
    {
        // Kiểm tra xem danh sách buttons có được gán hay không
        if (buttons == null || buttons.Count == 0)
        {
            Debug.LogWarning("Danh sách buttons chưa được gán trong Inspector!");
        }
        else
        {
            Debug.Log("Danh sách buttons đã được gán thành công!");
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.LogWarning("Chuột đã được click tại vị trí: " + Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast va chạm với: " + hit.collider.gameObject.name);

                foreach (GameObject button in buttons)
                {
                    if (hit.collider.gameObject == button || hit.collider.transform.IsChildOf(button.transform))
                    {
                        Debug.LogWarning("Đã chạm vào nút: " + button.name);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Không va chạm với đối tượng nào.");
            }
        }
    }
}
