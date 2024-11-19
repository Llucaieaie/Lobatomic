using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public PlayerMovementOnline playerMovement;
    //public PlayerWeapon playerWeapon;
    [HideInInspector] public PlayerData data = new PlayerData();

    TMP_Text nameTag;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement.dataManager = this;
        nameTag = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerValues(PlayerData newData)
    {
        SetName(newData.Name);
        transform.position = newData.Position;

        data = newData;
    }

    public void SetName(string newName)
    {
        data.Name = newName;
        nameTag.text = newName;
    }
}