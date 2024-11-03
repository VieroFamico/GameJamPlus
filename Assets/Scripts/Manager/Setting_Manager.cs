using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting_Manager : MonoBehaviour
{
    [Header("UI")]
    public GameObject settingPanel;
    public Button closeSetting;

    public Slider sfxSlider;                 // UI Slider for SFX volume
    public Slider bgmSlider;                 // UI Slider for BGM volume

    [Header("Audio")]
    public AudioMixer audioMixer;            // Reference to the main audio mixer
    public string sfxVolumeParameter = "SFXVolume";   // Name of the exposed parameter for SFX volume in the mixer
    public string bgmVolumeParameter = "BGMVolume";   // Name of the exposed parameter for BGM volume in the mixer

    private bool isInSetting;

    void Start()
    {
        closeSetting.onClick.AddListener(CloseSetting);

        // Load saved preferences and apply to the mixer and sliders
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolumeParameter, 0.75f); // Default to 75%
        bgmSlider.value = PlayerPrefs.GetFloat(bgmVolumeParameter, 0.75f);

        SetSFXVolume(sfxSlider.value);
        SetBGMVolume(bgmSlider.value);

        // Add listeners to sliders to update volume as they change
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    public void OpenSetting()
    {
        Cursor.visible = true;

        settingPanel.SetActive(true);
        isInSetting = true;

        if (Player_Entity.instance.player_Movement)
        {
            Player_Entity.instance.player_Movement.SetMovement(false);
        }
    }

    private void CloseSetting()
    {
        Cursor.visible = false;

        settingPanel.SetActive(false);
        isInSetting = false;

        if (Player_Entity.instance.player_Movement)
        {
            Player_Entity.instance.player_Movement.SetMovement(true);
        }
    }

    public void SetSFXVolume(float volume)
    {
        // Convert the slider value to a logarithmic scale for audio
        audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(sfxVolumeParameter, volume);  // Save to PlayerPrefs
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(bgmVolumeParameter, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(bgmVolumeParameter, volume);
    }

    private void OnDisable()
    {
        // Save preferences when the script or scene is closed
        PlayerPrefs.Save();
    }
}
