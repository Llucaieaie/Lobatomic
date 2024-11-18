using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOnline : MonoBehaviour
{
    public AudioSource walkAudio;
    public float maxSpeed;
    public Animator animator;
    public PowerUpManager powerUpManager;
    public ClientUDP clientUDP;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walkAudio.Play();
    }

    void Update()
    {
        // Capturamos el input del jugador
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Actualizamos la animación
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        // Cambiar el pitch del sonido dependiendo del estado del power-up
        if (powerUpManager != null)
        {
            walkAudio.pitch = powerUpManager.activeFrenesi ? 3 : 2;
        }
        walkAudio.mute = movement.magnitude == 0;
    }

    private void FixedUpdate()
    {
        // Movimiento del jugador usando Rigidbody2D
        Vector2 newPosition = rb.position + movement * maxSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Enviar posición al servidor
        if (movement.magnitude != 0)
        {
            clientUDP.SendPosition(newPosition);
        }
    }
}
