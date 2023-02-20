using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _comandi;
    [SerializeField] private GameObject _crediti;
    [SerializeField] private GameObject _info;

    //[SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _music;

    private AudioSource _audioSource;
    

    private void Start()
    {
        _startMenu.SetActive(true);
        _audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void PlayMusic()
    {
        _audioSource.PlayOneShot(_music);
    }
    
    /*public void MenuButton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _startMenu.SetActive(true);
        gameObject.SetActive(false);
    }*/

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(1); // 2 is the game
    }

    public void Comandi()
    {
        _comandi.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Crediti()
    {
        _crediti.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Info()
    {
        _info.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Back()
    {
        _startMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
