using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallImpulse : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.collider.CompareTag("Ball"))
        {
            Debug.Log("testestestestesteste");
            var opposite = -rb.velocity;
            rb.AddForce(opposite * Time.deltaTime);
        }
    }
}
