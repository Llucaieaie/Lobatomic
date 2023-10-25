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

    public GeneratePatient patient;
    public GameObject succedParticle;
    public MapGenerator mapGenerator;

    void Start()
    {
        min = 0;
        max = 100;
        current = Random.Range(33, 46);
        UpdateBar();
    }

    private void UpdateBar()
    {
        bar.fillAmount = current / 100;
        percentText = string.Format("{0}%", current);
        percent.text = percentText;
        patient.UpdateExpression(current);
    }

    public void destroySadTile()
    {
        current += 6;
        if (current > max) { current = max; Instantiate(succedParticle, patient.gameObject.transform.position, Quaternion.identity); StartCoroutine(mapGenerator.RestartLvl()); Regenerate(); }
        UpdateBar();
    }
    public void destroyHappyTile()
    {
        current -= 8;
        if (current < min) { current = min; StartCoroutine(mapGenerator.CleanUp()); }
        UpdateBar();
    }

    void Regenerate()
    {
        min = 0;
        max = 100;
        current = Random.Range(33, 46);
        patient.GenerateCharacters();
        UpdateBar();
    }
}
