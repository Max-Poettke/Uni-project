using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] private GameObject controlsUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private AudioSource sfxTest;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            controlsUI.SetActive(false);
            optionsUI.SetActive(false);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("WorldSelection");
    }

    public void Options()
    {
        optionsUI.SetActive(true);
    }
    
    public void Controls()
    {
        optionsUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void Credits()
    {
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void IncreaseVolumeMusic()
    {
        musicMixer.GetFloat("MasterVolume", out float currentVolume);
        musicMixer.SetFloat("MasterVolume",  currentVolume + 1f);
    }

    public void DecreaseVolumeMusic()
    {
        musicMixer.GetFloat("MasterVolume", out float currentVolume);
        musicMixer.SetFloat("MasterVolume",  currentVolume - 1f);
    }

    public void IncreaseVolumeSFX()
    {
        sfxMixer.GetFloat("SFXVolume", out float currentVolume);
        sfxMixer.SetFloat("SFXVolume",  currentVolume + 1f);
        sfxTest.Play();
    }

    public void DecreaseVolumeSFX()
    {
        sfxMixer.GetFloat("SFXVolume", out float currentVolume);
        sfxMixer.SetFloat("SFXVolume",  currentVolume - 1f);
        sfxTest.Play();
    }
}
