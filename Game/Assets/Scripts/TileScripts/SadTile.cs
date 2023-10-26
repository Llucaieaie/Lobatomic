using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadTile : Tile
{

    private void Start()
    {
        scoreController = GameObject.Find("ScoreController");
    }
    public override void OnExplosion()
    {
        //Update Happiness meter

        Score(50);
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
