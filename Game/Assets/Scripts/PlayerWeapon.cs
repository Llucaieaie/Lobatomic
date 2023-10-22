using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum direction
{
    UP, 
    DOWN, 
    LEFT, 
    RIGHT
}

public class PlayerWeapon : MonoBehaviour
{
    public GameObject weapon;
    public BoxCollider2D[] colliders;

    public Vector2 colliderSize   = new Vector2(3f, 2f);
    public Vector2 colliderOffset = new Vector2(0f, -1.75f);

    public float Attack_CD;
    [SerializeField] private float attackTimer = 0f;

    [SerializeField] private direction direction = direction.DOWN;

    void Start()
    {
        colliders = new BoxCollider2D[4];

        for(int col = 0; col < colliders.Length; col++)
        {
            
        }
    }

    void Update()
    {
        
    }
}
