using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public PlayerMovementOnline playerMovement;
    public PlayerWeaponOnline playerWeapon;

    [HideInInspector] public PlayerData data = new PlayerData();

    TMP_Text nameTag;
    public bool isControlled; // If true, user controls this and sends data to remote. If false, user doesn't control this and recieves data from remote.

    // Start is called before the first frame update
    void Start()
    {
        playerMovement.dataManager = this;
        playerWeapon.dataManager = this;

        nameTag = GetComponentInChildren<TMP_Text>();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Camera playerCam = GetComponentInChildren<Camera>();
        if (playerCam.enabled != isControlled) playerCam.enabled = isControlled;
    }

    public void SetPlayerValues(PlayerData newData)
    {
        // Change nametag
        if (newData.Name != data.Name) SetName(newData.Name);

        // Set position
        GetComponent<Rigidbody2D>().MovePosition(newData.Position);

        // Attack
        playerWeapon.Attack(newData.attackDirection);
        newData.attackDirection = AttackDirection.NONE;

        data = newData;
    }

    public void SetName(string newName)
    {
        data.Name = newName;
        nameTag.text = newName;
    }
}