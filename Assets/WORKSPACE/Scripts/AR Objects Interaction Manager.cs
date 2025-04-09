using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ARObjectInteractionManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> interactableObjects = new List<GameObject>();

    [SerializeField]
    private float maxRaycastDistance = 30f;

    [SerializeField]
    private LayerMask raycastLayerMask = -1; // Mặc định là tất cả các layer

    private Camera arCamera;

    void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnEnable()
    {
        Touch.onFingerDown += HandleFingerDown;
    }

    void OnDisable()
    {
        Touch.onFingerDown -= HandleFingerDown;
    }

    void Start()
    {
        arCamera = Camera.main;
        if (arCamera == null)
        {
            Debug.LogError("Không tìm thấy camera chính trong scene!");
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current == null)
        {
            Debug.LogError("Mouse.current là null");
        }
        else
        {
            // Debug.LogWarning("Mouse status: " + Mouse.current.leftButton.isPressed);
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.LogWarning("Click chuột đã được phát hiện!");
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                HandleTouch(mousePosition);
            }
        }
#endif
    }

    private void HandleFingerDown(Finger finger)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(finger.index))
        {
            Debug.LogWarning("Nhấn vào UI, không xử lý chạm.");
            return;
        }

        HandleTouch(finger.screenPosition);
    }

    private void HandleTouch(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, maxRaycastDistance, raycastLayerMask))
        {
            GameObject touchedObject = hit.collider.gameObject;
            foreach (GameObject obj in interactableObjects)
            {
                if (touchedObject == obj || touchedObject.transform.IsChildOf(obj.transform))
                {
                    Debug.LogWarning($"Đã chạm vào đối tượng: {obj.name}");
                    // TÌM VÀ GỌI PHƯƠNG THỨC TƯƠNG TÁC
                    // Tìm kiếm các component triển khai IInteractable
                    IInteractable[] interactables = touchedObject.GetComponents<IInteractable>();

                    // Nếu không có trên đối tượng được chạm vào, tìm trên đối tượng gốc
                    if (interactables.Length == 0 && touchedObject != obj)
                    {
                        interactables = obj.GetComponents<IInteractable>();
                    }

                    // Gọi phương thức OnInteract trên tất cả các component IInteractable
                    foreach (IInteractable interactable in interactables)
                    {
                        interactable.OnInteract();
                    }
                    break;
                }
            }
        }
    }

    // Phương thức để thêm đối tượng vào danh sách có thể tương tác
    public void AddInteractableObject(GameObject obj)
    {
        if (!interactableObjects.Contains(obj))
        {
            interactableObjects.Add(obj);
        }
    }

    // Phương thức để xóa đối tượng khỏi danh sách có thể tương tác
    public void RemoveInteractableObject(GameObject obj)
    {
        if (interactableObjects.Contains(obj))
        {
            interactableObjects.Remove(obj);
        }
    }

    // Phương thức để đặt lại toàn bộ danh sách
    public void SetInteractableObjects(List<GameObject> objects)
    {
        interactableObjects = new List<GameObject>(objects);
    }

    // Phương thức để lấy danh sách các đối tượng có thể tương tác
    public List<GameObject> GetInteractableObjects()
    {
        return interactableObjects;
    }
}