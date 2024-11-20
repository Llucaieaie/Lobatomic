using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    //public GameObject weapon;
    public PowerUpManager powerUpManager;
    public MapGenerator mapGenerator;
    public GameObject playerCam;
    public HappinessBar happiness;

    public AudioSource attackAudio;
    public AudioSource clashAudio;

    [Range(0.1f, 1f)] public float attackingTime;

    public float attackCoolDown;
    [SerializeField] private bool canAttack;

    private bool attackAudioFrenesi = false;

    [SerializeField] private AttackDirection direction;
    public GameObject[] weaponColliders;

    List<GameObject> tilesInside = new List<GameObject>(); //New list with the tiles inside weaponCollider

    //Start & Update -----------------------------------------------------------------------------------------
    void Start()
    {
        attackAudio = GetComponent<AudioSource>();

        canAttack = true;
        for (int i = 0; i < weaponColliders.Length; i++)
        {
            weaponColliders[i].SetActive(false);
        }
        playerCam = GameObject.Find("Gameplay Camera");
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetButton("UP")) && canAttack)
        {
            direction = AttackDirection.UP;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetButton("DOWN")) && canAttack)
        {
            direction = AttackDirection.DOWN;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetButton("LEFT")) && canAttack)
        {
            direction = AttackDirection.LEFT;
            StartCoroutine(Attack());
            canAttack = false;
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetButton("RIGHT")) && canAttack)
        {
            direction = AttackDirection.RIGHT;
            StartCoroutine(Attack());
            canAttack = false;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow)) { attackAudio.loop = false; attackAudioFrenesi = false; }
        if (Input.GetKeyUp(KeyCode.DownArrow)) { attackAudio.loop = false; attackAudioFrenesi = false; }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) { attackAudio.loop = false; attackAudioFrenesi = false; }
        if (Input.GetKeyUp(KeyCode.RightArrow)) { attackAudio.loop = false; attackAudioFrenesi = false; }

        if (powerUpManager != null)
        {
            if (!powerUpManager.activeFrenesi) { attackAudio.loop = false; attackAudioFrenesi = false; }
        }
    }

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
            case AttackDirection.UP:
                weaponColliders[0].SetActive(true);
                break;
            case AttackDirection.DOWN:
                weaponColliders[1].SetActive(true);
                break;
            case AttackDirection.LEFT:
                weaponColliders[2].SetActive(true);
                break;
            case AttackDirection.RIGHT:
                weaponColliders[3].SetActive(true);
                break;
            default:
                break;
        }
    }
    private void DisableTargetCollider()
    {
        switch (direction)
        {
            case AttackDirection.UP:
                weaponColliders[0].SetActive(false);
                break;
            case AttackDirection.DOWN:
                weaponColliders[1].SetActive(false);
                break;
            case AttackDirection.LEFT:
                weaponColliders[2].SetActive(false);
                break;
            case AttackDirection.RIGHT:
                weaponColliders[3].SetActive(false);
                break;
            default:
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
                    StartCoroutine(playerCam.GetComponent<CameraManager>().StartShake(2, 0.3f));
                    break;
                case 7:
                    happiness.destroySadTile();
                    collision.gameObject.GetComponent<SadTile>().OnExplosion();
                    StartCoroutine(playerCam.GetComponent<CameraManager>().StartShake(2, 0.3f));
                    break;
                case 8:
                    collision.gameObject.GetComponent<ExplosiveTile>().OnExplosion();
                    StartCoroutine(playerCam.GetComponent<CameraManager>().StartShake(5, 0.3f));
                    break;
                case 9:
                    powerUpManager.NewPowerUp();
                    collision.gameObject.GetComponent<PowerUpTile>().OnExplosion();
                    StartCoroutine(playerCam.GetComponent<CameraManager>().StartShake(2, 0.3f));
                    break;
                case 10:
                    collision.gameObject.GetComponent<NormalTile>().OnExplosion();
                    StartCoroutine(playerCam.GetComponent<CameraManager>().StartShake(2, 0.3f));
                    break;
                case 11:
                    float p = Random.Range(0.75f, 1.5f);
                    clashAudio.pitch = p;
                    clashAudio.Play();
                    break;

            }
        }
    }
}
