using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateButtonONOFF : MonoBehaviour
{
    [SerializeField] private Texture2D textureON;
    [SerializeField] private Texture2D textureOFF;

    private Renderer quadRenderer;
    private bool isON = false;

    void Start()
    {
        // Lấy component Renderer của Quad
        quadRenderer = GetComponent<Renderer>();

        // Đặt texture ban đầu là OFF
        if (quadRenderer != null && textureOFF != null)
        {
            // Sử dụng material hiện tại và thay đổi base map
            quadRenderer.material.mainTexture = textureOFF;
        }
    }

    public void OnInteract()
    {
        // Chuyển đổi trạng thái khi người dùng tương tác
        isON = !isON;

        // Đổi texture dựa trên trạng thái mới
        if (quadRenderer != null)
        {
            if (isON && textureON != null)
            {
                quadRenderer.material.mainTexture = textureON;
            }
            else if (!isON && textureOFF != null)
            {
                quadRenderer.material.mainTexture = textureOFF;
            }
        }

        Debug.LogWarning("Button switched to: " + (isON ? "ON" : "OFF"));

        // Phát hiệu ứng âm thanh hoặc animation (nếu cần)
        PlayButtonEffect();
    }

    private void PlayButtonEffect()
    {
        // Bạn có thể thêm code phát âm thanh hoặc hiệu ứng khác ở đây
        // Ví dụ:
        // if (buttonSound != null)
        //     AudioSource.PlayClipAtPoint(buttonSound, transform.position);
    }
}
