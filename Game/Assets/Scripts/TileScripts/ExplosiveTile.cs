using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTile : Tile
{
    [Range(0, 10)] public int explosionRadius;
    public ParticleSystem explosionParticle;
    public override void OnExplosion()
    {
        Explode();
        destroyParticle.Play();
        Destroy(gameObject);
    }

    void Explode()
    {
        //Destroy neighbouring tiles
        explosionParticle.Play();
    }
}
