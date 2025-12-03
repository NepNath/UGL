using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SettingMenuEvent : MonoBehaviour
{
    private UIDocument _document;

    private Button _Cancel;
    private Button _Aply;

    private DropdownField _DisplayResolution;
    private DropdownField _DisplayQuality;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _document = GetComponent<UIDocument>();

        _Cancel = _document.rootVisualElement.Q("CancelButton") as Button;
        _Cancel.RegisterCallback<ClickEvent>(OnCancelClick);

        _Aply = _document.rootVisualElement.Q("AplyButton") as Button;
        _Aply.RegisterCallback<ClickEvent>(OnAplyClick);

        InitDisplayResolution();
        InitDisplayQuality();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCancelClick(ClickEvent evt)
    {
        Debug.Log("Cancel");
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenuScene");
    }

    private void OnAplyClick(ClickEvent evt)
    {
        Debug.Log("Aply");
        var resolutiong = Screen.resolutions[_DisplayResolution.index];
        Screen.SetResolution(resolutiong.width, resolutiong.height, true);
        QualitySettings.SetQualityLevel(_DisplayQuality.index, true);
        SceneManager.LoadScene("MainMenuScene");
    }

    private void InitDisplayResolution()
    {
        _DisplayResolution = _document.rootVisualElement.Q<DropdownField>("DisplayResolution");
        _DisplayResolution.choices = Screen.resolutions.Select(Resolution => $"{Resolution.width}*{Resolution.height}").ToList();
       _DisplayResolution.index = Screen.resolutions
            .Select((Resolution, index) => (Resolution, index))
            .First((value) => value.Resolution.width == Screen.currentResolution.width && value.Resolution.height == Screen.currentResolution.height)
            .index;
    }

    private void InitDisplayQuality()
    {
        _DisplayQuality = _document.rootVisualElement.Q<DropdownField>("DisplayQuality");
        _DisplayQuality.choices = QualitySettings.names.ToList();
        _DisplayQuality.index = QualitySettings.GetQualityLevel();
    }

}
