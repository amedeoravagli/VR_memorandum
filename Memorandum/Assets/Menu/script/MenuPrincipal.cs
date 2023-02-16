using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void MenuButton()
    {
        SceneManager.LoadScene(1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2); // 2 is the game
    }

    public void Comandi()
    {
        SceneManager.LoadScene(3);
    }

    public void Crediti()
    {
        SceneManager.LoadScene(4);
    }

    public void Info()
    {
        SceneManager.LoadScene(5);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
