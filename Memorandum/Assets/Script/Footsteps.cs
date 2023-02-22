using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    [SerializeField] private AudioSource _audioWalk;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) && !_audioWalk.enabled)
        {
            _audioWalk.enabled= true;
        }
        else
        {
            _audioWalk.enabled = false;
        }
        
    }
}
