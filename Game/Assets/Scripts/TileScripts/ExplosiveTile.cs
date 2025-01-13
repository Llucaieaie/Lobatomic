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
        Instantiate(explosionParticle, transform.position, Quaternion.identity);

        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        GameObject ogm = GameObject.Find("Online Game Manager");

        yield return new WaitForEndOfFrame();

        RaycastHit2D[] tiles = Physics2D.CircleCastAll(transform.position, 2f, Vector2.zero, 0, layerMask);

        for (int i = 0; i < tiles.Length; i++)
        {
            Tile tile = tiles[i].transform.GetComponent<Tile>();

            Debug.Log(tile.name);

            bool destroyTile = false;
            if (tile != null && tile.transform.position != transform.position)
            {
                switch (tile.transform.gameObject.layer)
                {
                    case 6:
                        GameObject.Find("HappinessManager").GetComponent<HappinessBar>().destroyHappyTile();
                        destroyTile = true;
                        break;
                    case 7:
                        GameObject.Find("HappinessManager").GetComponent<HappinessBar>().destroySadTile();
                        destroyTile = true;
                        break;
                    case 8:
                    case 9:
                    case 10:
                        destroyTile = true;
                        break;
                    default:
                        break;
                }

                if (destroyTile)
                {
                    if (ogm != null)
                    {
                        Vector3Int tilePos = new Vector3Int((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z);
                        ogm.GetComponent<OnlineGameManager>().DestroyTileAtPosition(tilePos);
                    }
                    else tile.OnExplosion();
                }
            }
        }

        yield return new WaitForEndOfFrame();

        Destroy(this.gameObject);
    }
}