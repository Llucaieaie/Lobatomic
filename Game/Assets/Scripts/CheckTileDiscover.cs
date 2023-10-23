using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTileDiscover : MonoBehaviour
{
    [SerializeField][Range(1, 50)] float DisoverRad = 5;

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll((Vector2)transform.position, DisoverRad, Vector2.zero);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.gameObject.tag == "Tile" && hit[i].transform.gameObject.GetComponent<TileIsDiscovered>().isDiscovered)
            {
                hit[i].transform.gameObject.GetComponent<TileIsDiscovered>().TileIsDisovered();
            }
        }
    }
}
