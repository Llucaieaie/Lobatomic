using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileID = 0;
    public ParticleSystem destroyParticle;
    [HideInInspector]
    public GameObject scoreController;

    public virtual void OnExplosion()
    {
        if (destroyParticle != null) destroyParticle.Play();
        Destroy(gameObject);
    }


    public void Score(int score)
    {
        if (scoreController != null) scoreController.GetComponent<ScoreController>().AddScore(score);
    }
}
