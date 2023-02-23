using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{

    // Start is called before the first frame update

    [SerializeField] private GameObject _start;
    
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            //se rilasciamo il tasto esc andiamo in pausa
            //SceneManager.LoadScene(1);
            GoToStart();
        }
    }

    public void GoToStart()
    {

        _start.SetActive(true);
        gameObject.SetActive(false);
    }
}
