using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOnline : MonoBehaviour
{
    public AudioSource walkAudio;

    public float maxSpeed;

    float Xmove, Ymove;

    public Animator animator;

    public PowerUpManager powerUpManager;

    private void Start()
    {
        walkAudio.Play();
    }

    void Update()
    {
        Vector2 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        if (powerUpManager != null)
        {
            if (powerUpManager.activeFrenesi) walkAudio.pitch = 3;
            else walkAudio.pitch = 2;
        }

        if (movement.magnitude != 0) walkAudio.mute = false;
        else walkAudio.mute = true;

    }
}