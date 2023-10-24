using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyTile : Tile
{
    private void Start()
    {
        scoreController = GameObject.Find("ScoreController");
    }
    public override void OnExplosion()
    {
        //Update Happiness meter
        Score(-50);
        destroyParticle.Play();
        Destroy(this.gameObject);
    }
}
