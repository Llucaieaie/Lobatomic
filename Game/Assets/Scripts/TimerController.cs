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

        if (TimeCount <= 0)
        {
            Debug.Log("LOSE");
            //MapGenerator.GetComponent<MapGenerator>().CleanUp();
            Destroy(this.gameObject);
        }
    }
}
