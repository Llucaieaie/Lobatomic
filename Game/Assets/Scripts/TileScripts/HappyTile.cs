using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyTile : Tile
{
    public override void OnExplosion()
    {
        //Update Happiness meter
        destroyParticle.Play();
        Destroy(this.gameObject);
    }
}
