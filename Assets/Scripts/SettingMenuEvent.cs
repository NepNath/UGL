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
    private Button _Apply;

    private DropdownField _DisplayResolution;
    private DropdownField _DisplayQuality;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _document = GetComponent<UIDocument>();

        _Cancel = _document.rootVisualElement.Q("CancelButton") as Button;
        _Cancel.RegisterCallback<ClickEvent>(OnCancelClick);

        _Apply = _document.rootVisualElement.Q("ApplyButton") as Button;
        _Apply.RegisterCallback<ClickEvent>(OnApplyClick);

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

    private void OnApplyClick(ClickEvent evt)
    {
        Debug.Log("Apply");
        var resolution = Screen.resolutions[_DisplayResolution.index];
        Screen.SetResolution(resolution.width, resolution.height, true);
        QualitySettings.SetQualityLevel(_DisplayQuality.index, true);
        gameObject.SetActive(false);    
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
        _DisplayQuality = _document.rootVisualElement.Q<DropdownField>("Quality");
        _DisplayQuality.choices = QualitySettings.names.ToList();
        _DisplayQuality.index = QualitySettings.GetQualityLevel();
    }

}
