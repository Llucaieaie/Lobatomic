using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreVersusController : MonoBehaviour
{
    public float scoreMultiplier;
    [HideInInspector] public int score;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {
        score = 50;
    }
    void Update()
    {
        scoreText.text = score.ToString() + "%";
    }

    public void AddScoreP1(int s)
    {
        score += Mathf.RoundToInt(s * scoreMultiplier);
    }

    public void AddScoreP2(int s)
    {
        score -= Mathf.RoundToInt(s * scoreMultiplier);
    }
}
