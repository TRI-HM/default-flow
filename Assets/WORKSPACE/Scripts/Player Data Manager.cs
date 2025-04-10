using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
public class PlayerDataManager : MonoBehaviour
{

    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public string phone;
        public string email;
        public PlayerData(string name, string phone, string mail)
        {
            this.name = name;
            this.phone = phone;
            this.email = mail;
        }
    }
    public TMP_InputField playerName;
    public TMP_InputField playerPhone;
    public TMP_InputField playerMail;
    public Button confirmButton;
    public GameObject registerPanel;
    public TMP_Text welcomeText; // Thành phần UI để hiển thị thông tin

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }
    void OnConfirmButtonClick()
    {
        if (playerName == null || playerPhone == null || playerMail == null)
        {
            Debug.Log("Please enter complete information.");
            return;
        }
        string name = playerName.text;
        string phone = playerPhone.text;
        string mail = playerMail.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(mail))
        {
            Debug.Log("Please enter complete information.");
            return;
        }

        PlayerData playerData = new PlayerData(name, phone, mail);

        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("playerData", json);
        PlayerPrefs.Save();

        SocketInstance.Instance.RegisterEmitSocketEvent("userRegister", json);

        LoadAndDisplayPlayerData();
        registerPanel.SetActive(false);
    }
    void LoadAndDisplayPlayerData()
    {
        string jsonData = PlayerPrefs.GetString("playerData", string.Empty);
        if (jsonData != string.Empty)
        {
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            welcomeText.text = "Welcome, " + playerData.name;
        }
        else
        {
            welcomeText.text = "Welcome, Guest!\n.";
            Debug.LogWarning("Không tìm thấy file playerData.json trong PlayerPrefs.");
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("playerData");
        PlayerPrefs.Save();
        Debug.LogWarning("Dữ liệu playerData đã được xóa khỏi PlayerPrefs.");

    }
}
