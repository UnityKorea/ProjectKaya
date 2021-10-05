using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public PlanarReflections planarReflections;
    
    private Resolution[] _resolutions;
    private int[] _targetFrameRates;
    
    private void Start()
    {
        _targetFrameRates = new[] { 60, 40, 30, 20 };
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        var options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0 ; i < _resolutions.Length ; i++)
        {
            options.Add(_resolutions[i].width + " x " + _resolutions[i].height);
            if (_resolutions[i].width == Screen.currentResolution.width
                && _resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        var volumeValue = 0f;
        audioMixer.GetFloat("volume", out volumeValue);
        volumeSlider.value = (volumeValue+80f)/100f;
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume * 100f - 80f);
    }

    public void SetFPS(int fpsIndex)
    {
        Application.targetFrameRate = _targetFrameRates[fpsIndex];
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetReflectionResolution(int reflectionResolution)
    {
        if (planarReflections == null) return;
        
        planarReflections.m_settings.m_ResolutionMultiplier = (PlanarReflections.ResolutionMulltiplier)reflectionResolution;
        planarReflections.RefreshReflectionResolution();
    }
}
