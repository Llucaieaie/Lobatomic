using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTile : Tile
{
    public override void OnExplosion()
    {
        //Update Happiness meter
        //destroyParticle.Play();
        Destroy(this.gameObject);
    }
}