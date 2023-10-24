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
    public Camera camera;

    [Range(0.1f, 1f)] public float attackingTime;

    public float attackCoolDown;
    [SerializeField] private bool canAttack;

    [SerializeField] private direction direction;
    public BoxCollider2D[] colliders;

    List<GameObject> tilesInside = new List<GameObject>(); //New list with the tiles inside weaponCollider

    //Functions ----------------------------------------------------------------------------------------------
    public IEnumerator StartCooldown(float cd)
    {
        canAttack = false;
        yield return new WaitForSeconds(cd);
        canAttack = true;
    }

    private void EnableTargetCollider()
    {
        switch (direction)
        {
            case direction.UP:
                colliders[0].enabled=true;
                break;
            case direction.DOWN:
                colliders[1].enabled = true;
                break;
            case direction.LEFT:
                colliders[2].enabled = true;
                break;
            case direction.RIGHT:
                colliders[3].enabled = true;
                break;
        }
    }
    private void DisableTargetCollider()
    {
        switch (direction)
        {
            case direction.UP:
                colliders[0].enabled = false;
                break;
            case direction.DOWN:
                colliders[1].enabled = false;
                break;
            case direction.LEFT:
                colliders[2].enabled = false;
                break;
            case direction.RIGHT:
                colliders[3].enabled = false;
                break;
        }
    }

    private IEnumerator Attack()
    {
        EnableTargetCollider();
        yield return new WaitForSeconds(attackingTime);

        StartCoroutine(StartCooldown(attackCoolDown));

        DisableTargetCollider();
        tilesInside.Clear();
    }

    //Start & Update -----------------------------------------------------------------------------------------
    void Start()
    {
        canAttack = true;
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("UP")) && canAttack) 
        {
            direction = direction.UP;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetButtonDown("DOWN")) && canAttack) 
        {
            direction = direction.DOWN;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetButtonDown("LEFT")) && canAttack) 
        {
            direction = direction.LEFT;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetButtonDown("RIGHT")) && canAttack) 
        {
            direction = direction.RIGHT;
            StartCoroutine(Attack());
            canAttack = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 0 && collision.gameObject.tag == "Tile")
        {
            tilesInside.Add(collision.gameObject);
            Debug.Log(collision.name);


            //Call "OnExplosion" instantaneously 
            switch (collision.gameObject.layer)
            {
                case 6:
                    collision.gameObject.GetComponent<HappyTile>().OnExplosion();
                    StartCoroutine(camera.GetComponent<CameraManager>().StartShake(5, 0.3f));
                    break;
                case 7:
                    collision.gameObject.GetComponent<SadTile>().OnExplosion();
                    StartCoroutine(camera.GetComponent<CameraManager>().StartShake(5, 0.3f));
                    break;
                case 8:
                    collision.gameObject.GetComponent<ExplosiveTile>().OnExplosion();
                    StartCoroutine(camera.GetComponent<CameraManager>().StartShake(5, 0.3f));
                    break;
                case 9:
                    collision.gameObject.GetComponent<PowerUpTile>().OnExplosion();
                    StartCoroutine(camera.GetComponent<CameraManager>().StartShake(5, 0.3f));
                    break;
                case 10:
                    collision.gameObject.GetComponent<NormalTile>().OnExplosion();
                    StartCoroutine(camera.GetComponent<CameraManager>().StartShake(5, 0.3f));
                    break;

            }
        }
    }
}
