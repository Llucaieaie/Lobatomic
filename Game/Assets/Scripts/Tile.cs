using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public ParticleSystem destroyParticle;
    public Sprite undisoveredTile, disoveredTile;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = undisoveredTile;
    }

    public virtual void OnExplosion()
    {
        destroyParticle.Play();
        Destroy(gameObject);
    }

    public virtual void TileIsDisovered()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = disoveredTile;
    }
}
