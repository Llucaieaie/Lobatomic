using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum direction
{
    UP, 
    DOWN, 
    LEFT, 
    RIGHT
}

public class PlayerWeapon : MonoBehaviour
{
    //public GameObject weapon;
    public MapGenerator mapGenerator;

    public float attackCoolDown;
    [SerializeField] private bool canAttack;

    [SerializeField] private direction direction;
    public BoxCollider2D[] colliders;

    //Functions ----------------------------------------------------------------------------------------------
    public IEnumerator StartCooldown(float cd)
    {
        canAttack = false;
        yield return new WaitForSeconds(cd);
        canAttack = true;
    }

    private BoxCollider2D TargetCollider()
    {
        BoxCollider2D target = null;
        switch (direction)
        {
            case direction.UP:
                target = colliders[0];
                break;
            case direction.DOWN:
                target = colliders[1];
                break;
            case direction.LEFT:
                target = colliders[2];
                break;
            case direction.RIGHT:
                target = colliders[3];
                break;
        }
        return target;
    }

    private void Attack()
    {
        BoxCollider2D weaponCollider = TargetCollider();
        List<GameObject> tilesInside = new List<GameObject>(); //New list with the tiles inside weaponCollider

        TileStruct targetTile;

        //Add all the tiles that are inside the colldier in tilesInside --------------------------------------
        for (int i = 0; i < mapGenerator.tiles.Count; i++) 
        {
            targetTile = mapGenerator.tiles[i];
            if (targetTile.tileType != TileType.VOID && targetTile.tileType != TileType.INMUNE)
            {
                Debug.Log("This is not void or inmune");
                if (targetTile.tile.GetComponent<BoxCollider2D>().IsTouching(weaponCollider))
                {
                    Debug.Log("Passes the if");
                    tilesInside.Add(targetTile.tile);
                }
                else Debug.Log("Doesent pass the if");
            }
        }

        //Call "OnExplosion" on every tile of tilesInside ----------------------------------------------------
        for (int i = 0; i < tilesInside.Count; i++) 
        {
            Debug.Log("Trigger SendMessage");
            tilesInside[i].SendMessage("OnExplosion");
        }
    }

    //Start & Update -----------------------------------------------------------------------------------------
    void Start()
    {
        canAttack = true;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow) && canAttack) 
        {
            direction = direction.UP;
            Attack();
            StartCooldown(attackCoolDown);
        }
        if(Input.GetKey(KeyCode.DownArrow) && canAttack) 
        {
            direction = direction.DOWN;
            Attack();
            StartCooldown(attackCoolDown);
        }
        if(Input.GetKey(KeyCode.LeftArrow) && canAttack) 
        {
            direction = direction.LEFT;
            Attack();
            StartCooldown(attackCoolDown);
        }
        if(Input.GetKey(KeyCode.RightArrow) && canAttack) 
        {
            direction = direction.RIGHT;
            Attack();
            StartCooldown(attackCoolDown);
        }
    }
}
