using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyTile : Tile
{
    public override void OnExplosion()
    {
        //Update Happiness meter
        Score(-50);
        destroyParticle.Play();
        Destroy(this.gameObject);
    }
}
