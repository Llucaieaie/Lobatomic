using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadTile : Tile
{
    public override void OnExplosion()
    {
        //Update Happiness meter
        destroyParticle.Play();
        Destroy(gameObject);
    }
}
