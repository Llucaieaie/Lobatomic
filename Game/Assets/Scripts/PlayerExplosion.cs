using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosion : MonoBehaviour
{
    [Range(0,10)] public float explosionRadius;
    public ParticleSystem explosionParticle;
    public float explosionCooldown;
    bool explosionAvailable;

    void Start()
    {
        explosionAvailable = true;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Space) && explosionAvailable) {

            Explosion();

            //Start Cooldown
            StartCooldown();
        }
    }
    public IEnumerator StartCooldown()
    {
        explosionAvailable = false;

        yield return new WaitForSeconds(explosionCooldown);

        explosionAvailable = true;
    }
    void Explosion()
    {
        DestroyTiles();
        explosionParticle.Play();
    }
    void DestroyTiles()
    {

    }

    
}
