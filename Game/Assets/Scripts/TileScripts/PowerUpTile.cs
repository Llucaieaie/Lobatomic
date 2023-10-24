using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerUps
{
    UNKNOWN = -1,
    FRENESI,
    LAB_GOGGLES,
    DOUBLE_POINTS,
    STOP_TIME
}
public class PowerUpTile : Tile
{
    public override void OnExplosion()
    {
        //Update Happiness meter
        destroyParticle.Play();
        Destroy(this.gameObject);
    }

    void Frenesi()
    {

    }

    void LabGoggles()
    {

    }

    void DoublePoints()
    {

    }

    void StopTime()
    {

    }
}
