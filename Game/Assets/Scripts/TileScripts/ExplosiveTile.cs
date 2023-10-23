using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTile : Tile
{
    [Range(0, 10)] public int explosionRadius;
    public ParticleSystem explosionParticle;

    void Start()
    {
        OnExplosion();
    }

    public override void OnExplosion()
    {
        Explode();
        destroyParticle.Play();
        Destroy(this.gameObject, 0.5f);
    }

    void Explode()
    {
        //Destroy neighbouring tiles
        explosionParticle.Play();
    }

}
