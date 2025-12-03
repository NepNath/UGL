using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    private Button _Start;
    private Button _Settings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _document = GetComponent<UIDocument>();

        _Start = _document.rootVisualElement.Q("StartButton") as Button;
        _Start.RegisterCallback<ClickEvent>(OnStartClick);

        _Settings = _document.rootVisualElement.Q("SettingButton") as Button;
        _Settings.RegisterCallback<ClickEvent>(OnSettingClick);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSettingClick(ClickEvent evt)
    {
        Debug.Log("Setting");
        SceneManager.LoadScene("SettingMenuScene");
    }

    private void OnStartClick(ClickEvent evt)
    {
        Debug.Log("Start");
        SceneManager.LoadScene("GameScene");
    }
}
