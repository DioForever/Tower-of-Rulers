using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sliderVolume;
    public Slider sliderSensitivity;

    private const string VolumeKey = "Volume";
    private const string SensitivityKey = "Sensitivity";

    private void Start()
    {
        LoadSettings();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);
        SaveSettings();
    }

    public void SetSensitivity(float sensitivity)
    {
        SaveSettings();
    }

    private void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f); 
        float savedSensitivity = PlayerPrefs.GetFloat(SensitivityKey, 0.5f); 

        // Set sliders based on loaded values
        sliderVolume.value = savedVolume;
        sliderSensitivity.value = savedSensitivity;

        // Apply loaded volume to AudioMixer
        audioMixer.SetFloat("MainVolume", savedVolume);
    }

    private void SaveSettings()
    {
        // Save current values to PlayerPrefs
        PlayerPrefs.SetFloat(VolumeKey, sliderVolume.value);
        PlayerPrefs.SetFloat(SensitivityKey, sliderSensitivity.value);

        // This line is essential to persist the PlayerPrefs data between sessions
        PlayerPrefs.Save();
    }
}