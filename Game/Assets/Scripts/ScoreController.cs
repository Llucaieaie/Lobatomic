using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public int scoreMultiplier;
    int score;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {
        score = 0;
    }
    void Update()
    {
        scoreText.text = score.ToString();
    }

    public void AddScore(int s)
    {
        score += s * scoreMultiplier;
    }
}
