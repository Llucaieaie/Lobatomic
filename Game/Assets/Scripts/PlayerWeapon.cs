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
    public PowerUpManager powerUpManager;
    public MapGenerator mapGenerator;
    public GameObject camera;
    public HappinessBar happiness;

    private AudioSource attackAudio;

    [Range(0.1f, 1f)] public float attackingTime;

    public float attackCoolDown;
    [SerializeField] private bool canAttack;

    private bool attackAudioFrenesi = false;

    [SerializeField] private direction direction;
    public BoxCollider2D[] colliders;

    List<GameObject> tilesInside = new List<GameObject>(); //New list with the tiles inside weaponCollider

    //Functions ----------------------------------------------------------------------------------------------
    public IEnumerator StartCooldown(float cd)
    {
        canAttack = false;
        yield return new WaitForSecondsRealtime(cd);
        canAttack = true;
    }

    private void EnableTargetCollider()
    {
        switch (direction)
        {
            case direction.UP:
                colliders[0].enabled = true;
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
        attackAudio.pitch = Random.Range(0.75f, 1.5f);

        if (!powerUpManager.activeFrenesi) attackAudio.Play();
        if (powerUpManager.activeFrenesi && !attackAudioFrenesi) { attackAudio.loop = true; attackAudio.Play(); attackAudioFrenesi = true; }
        
        EnableTargetCollider();
        yield return new WaitForSecondsRealtime(attackingTime);

        StartCoroutine(StartCooldown(attackCoolDown));

        DisableTargetCollider();
        tilesInside.Clear();
    }

    //Start & Update -----------------------------------------------------------------------------------------
    void Start()
    {
        attackAudio = GetComponent<AudioSource>();

        canAttack = true;
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    void Update()
    {
        if((Input.GetKey(KeyCode.UpArrow) || Input.GetButton("UP")) && canAttack) 
        {
            direction = direction.UP;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if((Input.GetKey(KeyCode.DownArrow) || Input.GetButton("DOWN")) && canAttack) 
        {
            direction = direction.DOWN;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetButton("LEFT")) && canAttack) 
        {
            direction = direction.LEFT;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetButton("RIGHT")) && canAttack) 
        {
            direction = direction.RIGHT;
            StartCoroutine(Attack());
            canAttack = false;
        }

        if(Input.GetKeyUp(KeyCode.UpArrow))    { attackAudio.loop = false; attackAudioFrenesi = false; }
        if(Input.GetKeyUp(KeyCode.DownArrow))  { attackAudio.loop = false; attackAudioFrenesi = false; }
        if(Input.GetKeyUp(KeyCode.LeftArrow))  { attackAudio.loop = false; attackAudioFrenesi = false; }
        if(Input.GetKeyUp(KeyCode.RightArrow)) { attackAudio.loop = false; attackAudioFrenesi = false; }

        if (!powerUpManager.activeFrenesi) { attackAudio.loop = false; attackAudioFrenesi = false; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 0 && collision.gameObject.tag == "Tile")
        {
            tilesInside.Add(collision.gameObject);
            Debug.Log(collision.name);

            //Call "OnExplosion" instantaneously 
            switch (collision.gameObject.layer)
            {
                case 6:
                    happiness.destroyHappyTile();
                    collision.gameObject.GetComponent<HappyTile>().OnExplosion();
                    StartCoroutine(camera.GetComponent<CameraManager>().StartShake(5, 0.3f));
                    break;
                case 7:
                    happiness.destroySadTile();
                    collision.gameObject.GetComponent<SadTile>().OnExplosion();
                    StartCoroutine(camera.GetComponent<CameraManager>().StartShake(5, 0.3f));
                    break;
                case 8:
                    collision.gameObject.GetComponent<ExplosiveTile>().OnExplosion();
                    StartCoroutine(camera.GetComponent<CameraManager>().StartShake(20, 0.3f));
                    break;
                case 9:
                    powerUpManager.NewPowerUp();
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
