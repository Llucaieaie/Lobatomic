using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HappinessBar : MonoBehaviour
{
    public Image bar;
    public TextMeshProUGUI percent;

    private int min;
    private int max;
    public float current;
    public string percentText;

    void Start()
    {
        min = 0;
        max = 100;
        current = Random.Range(33, 46);
        UpdateBar();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateBar()
    {
        bar.fillAmount = current / 100;
        percentText = string.Format("{0}%", current);
        percent.text = percentText;
    }

    public void destroySadTile()
    {
        current += 6;
        if (current > max) { current = max; }
        UpdateBar();
    }
    public void destroyHappyTile()
    {
        current -= 8;
        if (current < min) { current = min; }
        UpdateBar();
    }
}
