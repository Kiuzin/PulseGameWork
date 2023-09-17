using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float movement;
    [SerializeField] private float jumpforce = 5f;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement * speed, rb.velocity.y);        
    }


}
