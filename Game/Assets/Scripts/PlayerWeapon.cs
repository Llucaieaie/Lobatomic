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

        //foreach (GameObject tile in mapGenerator.tiles) //Add all the tiles that are inside the colldier in tilesInside
        //{
        //    if (tile.GetComponent<BoxCollider2D>().IsTouching(weaponCollider)) { tilesInside.Add(tile); }
        //}
        foreach (GameObject targetTile in tilesInside) //Call "OnExplosion" on every tile of tilesInside
        {
            targetTile.SendMessage("OnExplosion");
        }
    }

    //Start & Update -----------------------------------------------------------------------------------------
    void Start()
    {
        mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();

        colliders = new BoxCollider2D[4];

        colliders[0] = GameObject.FindGameObjectWithTag("Col_UP").GetComponent<BoxCollider2D>();
        colliders[1] = GameObject.FindGameObjectWithTag("Col_DOWN").GetComponent<BoxCollider2D>();
        colliders[2] = GameObject.FindGameObjectWithTag("Col_LEFT").GetComponent<BoxCollider2D>();
        colliders[3] = GameObject.FindGameObjectWithTag("Col_RIGHT").GetComponent<BoxCollider2D>();
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
