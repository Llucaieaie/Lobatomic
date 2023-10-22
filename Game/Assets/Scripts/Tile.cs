using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public ParticleSystem destroyParticle;

    public virtual void OnExplosion()
    {
        destroyParticle.Play();
        Destroy(gameObject);
    }
}
