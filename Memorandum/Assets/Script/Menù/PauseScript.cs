using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    bool isPaused;
    public GameObject pnlPause;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //se rilasciamo il tasto esc andiamo in pausa
            ChangePauseStatus();
            
        }
    }

    void UpdateGamePause()
    {
        if (isPaused)
        {
            //ferma il gioco
            Time.timeScale = 0;

        }
        else
        {
            //riavvia il tempo
            Time.timeScale = 1;

        }
        pnlPause.SetActive(isPaused);
    }

    public void ChangePauseStatus()
    {
        isPaused = !isPaused;
        UpdateGamePause();
    }
}
