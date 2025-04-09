using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient; 
using System; 

public class SocketInstance : MonoBehaviour
{
    public static SocketInstance Instance { get; private set; } // Singleton instance
    public SocketIOClient.SocketIO socket { get; private set; } // Socket.IO client
    public string serverUrl = "http://localhost:3000"; // Đổi thành URL server của bạn
    private Dictionary<string, Action<SocketIOResponse>> eventListeners = new Dictionary<string, Action<SocketIOResponse>>();
    private Dictionary<string, object> emitQueue = new Dictionary<string, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không bị hủy khi đổi Scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeSocket();
    }

    private async void InitializeSocket()
    {
        socket = new SocketIOClient.SocketIO(serverUrl, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        // Lắng nghe sự kiện kết nối
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Socket.IO Connected!");
            ProcessEmitQueue(); // Gửi các sự kiện đã được đăng ký trước khi kết nối
            RegisterAllListeners(); // Đăng ký lại tất cả sự kiện đã lắng nghe trước đó
        };

        // Lắng nghe sự kiện ngắt kết nối
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("Socket.IO Disconnected!");
        };

        await socket.ConnectAsync(); // Kết nối đến server
    }

    // 🎯 Đăng ký sự kiện lắng nghe từ server
    public void RegisterOnSocketEvent(string eventName, Action<SocketIOResponse> callback)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = callback;
            if (socket.Connected)
            {
                socket.On(eventName, callback);
                Debug.Log($"👂 Registered event: {eventName}");
            }
        }
        else
        {
            Debug.LogWarning($"⚠️ Event {eventName} is already registered!");
        }
    }
    // 🛠 Đăng ký lại tất cả các sự kiện khi socket bị ngắt kết nối rồi kết nối lại
    private void RegisterAllListeners()
    {
        foreach (var eventItem in eventListeners)
        {
            socket.On(eventItem.Key, eventItem.Value);
            Debug.Log($"🔄 Re-registered event: {eventItem.Key}");
        }
    }
    // 📤 Gửi sự kiện lên server
    public void RegisterEmitSocketEvent(string eventName, object data)
    {
        if (socket.Connected)
        {
            socket.EmitAsync(eventName, data);
            Debug.Log($"📤 Sent event: {eventName} with data: {data}");
        }
        else
        {
            emitQueue[eventName] = data; // Lưu lại để gửi sau khi kết nối
            Debug.LogWarning($"⏳ Socket chưa kết nối! Lưu event {eventName} để gửi sau.");
        }
    }

    // ⏳ Gửi lại các sự kiện đã lưu khi kết nối thành công
    private async void ProcessEmitQueue()
    {
        foreach (var eventItem in emitQueue)
        {
            await socket.EmitAsync(eventItem.Key, eventItem.Value);
            Debug.Log($"📤 Sent queued event: {eventItem.Key} with data: {eventItem.Value}");
        }
        emitQueue.Clear();
    }

    // Hàm ngắt kết nối khi game đóng
    private async void OnApplicationQuit()
    {
        if (socket != null)
        {
            await socket.DisconnectAsync();
        }
    }
}
