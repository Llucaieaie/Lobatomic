using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    public float maxSpeed;
    public float acceleration;

    public bool canMove;

    [SerializeField] private bool isMovingX = false;
    [SerializeField] private bool isMovingY = false;

    [SerializeField] private Vector3 directionVector = Vector3.zero;

    float Xmove, Ymove;

    public Animator animator;

    private void Start()
    {
        canMove = true;
    }

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

        Vector2 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + movement * maxSpeed * Time.deltaTime);
    }
}