using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    public float maxSpeed = 10f;
    public float acceleration = 0.05f;

    [SerializeField] private bool isMovingX = false;
    [SerializeField] private bool isMovingY = false;

    [SerializeField] private Vector3 directionVector = Vector3.zero;

    float Xmove, Ymove;

    void Update()
    {
        Vector3 auxVec = Vector3.zero;

        //Check inputs ---------------------------------------------------------------------------------------------------
        //--- X axis ---
        if      (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) { auxVec.x += -acceleration; isMovingX = true; }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) { auxVec.x +=  acceleration; isMovingX = true; }
        else isMovingX = false;

        //--- Y Axis ---
        if      (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) { auxVec.y += -acceleration; isMovingY = true; }
        else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) { auxVec.y +=  acceleration; isMovingY = true; }
        else isMovingY = false;

        //If no inputs then accelerate to 0 ------------------------------------------------------------------------------
        if (isMovingX == false)
        {
            if (directionVector.x < -acceleration) auxVec.x =  acceleration;
            if (directionVector.x >  acceleration) auxVec.x = -acceleration;
        }
        if (isMovingY == false)
        {
            if (directionVector.y < -acceleration) auxVec.y =  acceleration;
            if (directionVector.y >  acceleration) auxVec.y = -acceleration;
        }

        //Modify directionVector -----------------------------------------------------------------------------------------
        directionVector += auxVec;

        //Control that speed doesn't surpass maxSpeed --------------------------------------------------------------------
        if (directionVector.x >  maxSpeed) directionVector.x =  maxSpeed;
        if (directionVector.x < -maxSpeed) directionVector.x = -maxSpeed;
        if (directionVector.y >  maxSpeed) directionVector.y =  maxSpeed;
        if (directionVector.y < -maxSpeed) directionVector.y = -maxSpeed;

        //Control that speed gets to 0 properly --------------------------------------------------------------------------
        if (isMovingX == false && directionVector.x > -acceleration && directionVector.x < acceleration) directionVector.x = 0;
        if (isMovingY == false && directionVector.y > -acceleration && directionVector.y < acceleration) directionVector.y = 0;

        //transform.position += directionVector * Time.deltaTime;

        Xmove = Input.GetAxis("Horizontal") * maxSpeed * Time.deltaTime;
        Ymove = Input.GetAxis("Vertical") * maxSpeed * Time.deltaTime;
        //transform.position += new Vector3(Xmove, Ymove, 0);

        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + new Vector2(Xmove, Ymove));
    }
}