using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOnline : MonoBehaviour
{
    public AudioSource walkAudio;
    public float maxSpeed;
    public Animator animator;
    public PowerUpManager powerUpManager;
    [HideInInspector] public ServerUDP serverUDP;
    [HideInInspector] public ClientUDP clientUDP;

    public bool isControlled; // If true, user controls this and sends position to remote. If false, user doesn't control this and recieves position from remote.

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walkAudio.Play();
        serverUDP = GameObject.Find("ServerUDP").GetComponent<ServerUDP>();
        clientUDP = GameObject.Find("ClientUDP").GetComponent<ClientUDP>();
    }

    void Update()
    {
        if (isControlled)
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
    }

    private void FixedUpdate()
    {
        if (isControlled)
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
}
