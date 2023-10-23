using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float TimeCount;
    [SerializeField] TextMeshProUGUI timer;

    // Update is called once per frame
    void Update()
    {
        TimeCount -= Time.deltaTime;
        
        timer.text = Mathf.RoundToInt(TimeCount).ToString();

        if (TimeCount <= 0)
        {
            Debug.Log("LOSE");
            GameObject.Find("MapGenerator").GetComponent<MapGenerator>().CleanUp();
        }
    }
}
