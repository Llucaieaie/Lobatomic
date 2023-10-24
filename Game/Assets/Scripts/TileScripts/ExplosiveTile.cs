using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTile : Tile
{
    [Range(0, 10)] public float explosionRadius;
    public ParticleSystem explosionParticle;
    public LayerMask layerMask;

    private void Start()
    {
        scoreController = GameObject.Find("ScoreController");
    }
    public override void OnExplosion()
    {
        Score(20);
        //Destroy neighbouring tiles
        destroyParticle.Play();
        explosionParticle.Play();

        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForEndOfFrame();

        RaycastHit2D[] tiles = Physics2D.CircleCastAll(transform.position, 2f, Vector2.zero, 0, layerMask);

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].transform.gameObject.transform.position != transform.position)
            {
                switch (tiles[i].transform.gameObject.layer)
                {
                    case 6:
                        tiles[i].transform.gameObject.GetComponent<HappyTile>().OnExplosion();
                        break;
                    case 7:
                        tiles[i].transform.gameObject.GetComponent<SadTile>().OnExplosion();
                        break;
                    case 8:
                        tiles[i].transform.gameObject.GetComponent<ExplosiveTile>().OnExplosion();
                        break;
                    case 9:
                        //powerUp
                        break;
                    case 10:
                        tiles[i].transform.gameObject.GetComponent<NormalTile>().OnExplosion();
                        break;
                }
            }
        }

        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

}
