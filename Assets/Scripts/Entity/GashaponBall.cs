using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GashaponBall : MonoBehaviour
{
    Rigidbody2D rb;

    Vector3 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        lastVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GashaponMachine")
        {
            var speed = lastVelocity.magnitude;
            // var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            // random direction
            var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            rb.velocity = direction * Mathf.Max(speed, 0f);
        }
    }
}
