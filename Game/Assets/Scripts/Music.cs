using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public AudioSource[] music;
    void Start()
    {
        music[0].Play();
    }

    
}
