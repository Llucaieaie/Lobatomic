using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponOnline : MonoBehaviour
{
    public float attackingTime;
    public float attackCoolDown;

    public AudioSource attackAudio;
    public AudioSource clashAudio;

    public GameObject[] weaponColliders;

    float lastAttackTime = 0f;
    public bool canAttack;

    [HideInInspector] public PlayerDataManager dataManager;

    //Start & Update -----------------------------------------------------------------------------------------
    void Start()
    {
        attackAudio = GetComponent<AudioSource>();
        canAttack = true;

        for (int i = 0; i < weaponColliders.Length; i++)
        {
            weaponColliders[i].SetActive(false);
        }
    }

    void Update()
    {
        if (dataManager.isControlled)
        {
            AttackDirection direction = AttackDirection.NONE;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetButton("UP")) direction = AttackDirection.UP;
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetButton("DOWN")) direction = AttackDirection.DOWN;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetButton("LEFT")) direction = AttackDirection.LEFT;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetButton("RIGHT")) direction = AttackDirection.RIGHT;

            Attack(direction);
            dataManager.data.attackDirection = direction;
        }

        if (!canAttack && (Time.time - lastAttackTime > attackCoolDown))
        {
            canAttack = true;
        }
    }

    /// <summary>
    /// Enable / Disable attack colliders
    /// </summary>
    void EnableTargetCollider(AttackDirection direction)
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
    void DisableTargetCollider(AttackDirection direction)
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

    /// <summary>
    /// Attack
    /// </summary>
    public void Attack(AttackDirection direction)
    {
        if (canAttack && direction != AttackDirection.NONE)
        {
            canAttack = false;
            lastAttackTime = Time.time;
            attackAudio.pitch = Random.Range(0.75f, 1.5f);
            attackAudio.loop = false;

            StartCoroutine(AttackCoroutine(direction));
        }
    }
    IEnumerator AttackCoroutine(AttackDirection direction)
    {
        EnableTargetCollider(direction);
        yield return new WaitForSecondsRealtime(attackingTime);
        DisableTargetCollider(direction);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 0 && collision.gameObject.tag == "Tile")
        {
            //Destroy tiles
            switch (collision.gameObject.layer)
            {
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    collision.GetComponent<Tile>().OnExplosion();
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