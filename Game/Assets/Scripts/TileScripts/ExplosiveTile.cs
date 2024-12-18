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
        //Instantiate(destroyParticle, transform.position, Quaternion.identity);

        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        GameObject ogm = GameObject.Find("Online Game Manager");
        List<GameObject> tiles = new List<GameObject>();

        yield return new WaitForEndOfFrame();

        RaycastHit2D[] hitList = Physics2D.CircleCastAll(transform.position, 2f, Vector2.zero, 0, layerMask);
        for (int i = 0; i < hitList.Length; i++) tiles.Add(hitList[i].transform.gameObject);

            for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].transform.gameObject.transform.position != transform.position)
            {
                switch (tiles[i].transform.gameObject.layer)
                {
                    case 6:
                        GameObject.Find("HappinessManager").GetComponent<HappinessBar>().destroyHappyTile();
                        break;
                    case 7:
                        GameObject.Find("HappinessManager").GetComponent<HappinessBar>().destroySadTile();
                        break;
                    default: 
                        break;
                }

                Tile tileComp = tiles[i].transform.gameObject.GetComponent<Tile>();
                if (ogm != null) ogm.GetComponent<OnlineGameManager>().DestroyTileByID(tileComp.tileID);
                else tileComp.OnExplosion();
            }
        }

        yield return new WaitForEndOfFrame();

        Destroy(this.gameObject);
    }

}
