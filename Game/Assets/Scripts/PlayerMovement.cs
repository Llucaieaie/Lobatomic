using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum movDirection
{
    IDLE,
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 1f;

    //[SerializeField] private movDirection MovementType = movDirection.IDLE;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        Vector3 directionVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) directionVector.y = moveSpeed;
        if (Input.GetKey(KeyCode.S)) directionVector.y = -moveSpeed;
        if (Input.GetKey(KeyCode.A)) directionVector.x = -moveSpeed;
        if (Input.GetKey(KeyCode.D)) directionVector.x = moveSpeed;

        transform.position += directionVector * Time.deltaTime;
    }
}
