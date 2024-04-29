using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    const string MixerMusic = "MusicVol";
    const string MixerSFX = "SFXVol";

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MixerMusic, Mathf.Log10(value) * 20);
    }

    void SetSfxVolume(float value)
    {
        mixer.SetFloat(MixerSFX, Mathf.Log10(value) * 20);
    }
}
