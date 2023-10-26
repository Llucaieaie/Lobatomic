using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public ParticleSystem destroyParticle;
    [HideInInspector]
    public GameObject scoreController;

    public virtual void OnExplosion()
    {
        destroyParticle.Play();
        Destroy(gameObject);
    }


    public void Score(int score)
    {
        scoreController.GetComponent<ScoreController>().AddScore(score);
    }
}
