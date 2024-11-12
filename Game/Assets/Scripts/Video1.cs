using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Video1 : MonoBehaviour
{
    [SerializeField] GameObject video2;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<VideoPlayer>().isPaused)
        {
            video2.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
