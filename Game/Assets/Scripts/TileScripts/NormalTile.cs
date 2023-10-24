using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : Tile
{
    public override void OnExplosion()
    {
        //Update Happiness meter
        Score(10);
        destroyParticle.Play();
        Destroy(this.gameObject);
    }
}
