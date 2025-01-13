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
        Debug.Log("HOLA?");

        GameObject ogm = GameObject.Find("Online Game Manager");

        // Wait 2 frames because of OnlineGameManager
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        RaycastHit2D[] hitList = Physics2D.CircleCastAll(transform.position, 2f, Vector2.zero, 0, layerMask);
        for (int i = 0; i < hitList.Length; i++)
        {
            Debug.Log("QUE");

            Tile tile = hitList[i].transform.GetComponent<Tile>();
            bool destroyTile = false;
            if (tile.transform.gameObject.transform.position != transform.position)
            {
                Debug.Log("BBBB");
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
                    Debug.Log("AAAAA");
                    if (ogm != null)
                    {
                        //ogm.GetComponent<OnlineGameManager>().DestroyTileByID(tile.tileID);
                    }
                    else tile.OnExplosion();
                }
            }
        }

        yield return new WaitForEndOfFrame();

        Destroy(this.gameObject);
    }
}