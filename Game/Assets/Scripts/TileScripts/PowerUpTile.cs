using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTile : Tile
{

    private void Start()
    {
        scoreController = GameObject.Find("ScoreController");
    }
    public override void OnExplosion()
    {

        Score(20);
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}