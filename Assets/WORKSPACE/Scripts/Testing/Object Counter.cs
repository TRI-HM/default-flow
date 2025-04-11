using UnityEngine;
using System.Collections;
using TMPro;
using System.Linq;

/// <summary>
/// Script giám sát hiệu suất và số lượng đối tượng trong scene.
/// Cho phép theo dõi tác động của Occlusion Culling và các kỹ thuật tối ưu khác.
/// </summary>
public class ObjectCounter : MonoBehaviour
{
    // Tham chiếu đến text UI để hiển thị thông tin (nếu có)
    [SerializeField] private TextMeshProUGUI statsText;

    // Các biến để theo dõi hiệu suất
    private float deltaTime = 0.0f;
    private float updateInterval = 0.5f; // Cập nhật mỗi 0.5 giây
    private float nextUpdateTime = 0.0f;

    // Biến theo dõi đối tượng
    private int totalObjects = 0;
    private int renderedObjects = 0;
    private int culledObjects = 0;

    // Biến cài đặt
    [SerializeField] private bool showInEditor = true;
    [SerializeField] private bool logToConsole = true;
    [SerializeField] private KeyCode toggleKey = KeyCode.F2;

    // Các tùy chọn hiển thị
    private bool showStats = true;
    private GUIStyle guiStyle = new GUIStyle();

    void Start()
    {
        // Cài đặt ban đầu cho GUI
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.white;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.alignment = TextAnchor.UpperLeft;
        guiStyle.wordWrap = true;

        // Đảm bảo script này không bị hủy khi chuyển scene
        DontDestroyOnLoad(this.gameObject);

        // Bắt đầu kiểm tra liên tục
        StartCoroutine(CountObjectsRoutine());
    }

    void Update()
    {
        // Tính toán FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Kiểm tra phím tắt để bật/tắt hiển thị
        if (Input.GetKeyDown(toggleKey))
        {
            showStats = !showStats;
        }
    }

    IEnumerator CountObjectsRoutine()
    {
        while (true)
        {
            // Chờ đến thời điểm cập nhật tiếp theo
            if (Time.time > nextUpdateTime)
            {
                CountObjects();
                nextUpdateTime = Time.time + updateInterval;
            }

            yield return null;
        }
    }

    void CountObjects()
    {
        // Đếm tổng số đối tượng trong scene
        totalObjects = GameObject.FindObjectsOfType<GameObject>().Length;

        // Đếm số đối tượng đang được render bởi camera chính
        renderedObjects = 0;
        culledObjects = 0;

        // Lấy tất cả Renderer trong scene
        Renderer[] allRenderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            // Kiểm tra xem đối tượng có đang được camera chính render không
            if (renderer.isVisible)
            {
                renderedObjects++;
            }
            else
            {
                culledObjects++;
            }
        }

        // Cập nhật UI nếu có
        UpdateStatsDisplay();

        // In thông tin ra console nếu được yêu cầu
        if (logToConsole)
        {
            string logMessage = $"[Hiệu suất] FPS: {CalculateFPS():0.0} | " +
                               $"Tổng đối tượng: {totalObjects} | " +
                               $"Đang render: {renderedObjects} | " +
                               $"Đã culled: {culledObjects}";
            Debug.Log(logMessage);
        }
    }

    void UpdateStatsDisplay()
    {
        // Nếu có tham chiếu đến UI text, cập nhật nó
        if (statsText != null)
        {
            statsText.text = $"FPS: {CalculateFPS():0.0}\n" +
                            $"Tổng đối tượng: {totalObjects}\n" +
                            $"Đang render: {renderedObjects}\n" +
                            $"Đã culled: {culledObjects}\n" +
                            $"Tỷ lệ culling: {(totalObjects > 0 ? (float)culledObjects / totalObjects * 100 : 0):0.0}%";
        }
    }

    float CalculateFPS()
    {
        return 1.0f / deltaTime;
    }

    void OnGUI()
    {
        // Chỉ hiển thị trong editor nếu được cài đặt và không có UI text
        if ((Application.isEditor && !showInEditor) || !showStats)
            return;

        // Nếu không có UI text được gán, hiển thị thông qua OnGUI
        if (statsText == null)
        {
            float fps = CalculateFPS();
            string text = $"FPS: {fps:0.0}\n" +
                         $"Tổng đối tượng: {totalObjects}\n" +
                         $"Đang render: {renderedObjects}\n" +
                         $"Đã culled: {culledObjects}\n" +
                         $"Tỷ lệ culling: {(totalObjects > 0 ? (float)culledObjects / totalObjects * 100 : 0):0.0}%";

            // Tạo background tối để dễ đọc
            GUI.Box(new Rect(10, 10, 200, 100), "");
            GUI.Label(new Rect(10, 10, 200, 100), text, guiStyle);
        }
    }
}