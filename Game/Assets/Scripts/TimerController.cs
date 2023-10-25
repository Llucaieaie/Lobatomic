using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float TimeCount;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] GameObject MapGenerator;

    // Update is called once per frame
    void Update()
    {
        TimeCount -= Time.deltaTime;
        
        timer.text = Mathf.RoundToInt(TimeCount).ToString();

        if(Input.GetKeyDown(KeyCode.Escape)) TimeCount = 0;
        if (TimeCount <= 0)
        {
            TimeCount = 0;
            CleanMap();
        }
    }

    private void CleanMap()
    {
        Debug.Log("LOSE");

        StartCoroutine(MapGenerator.GetComponent<MapGenerator>().CleanUp());
        Destroy(this.gameObject);
    }
}
