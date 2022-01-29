using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    // Rigidbody contains functions to apply physical movement
    public Rigidbody2D Rigidbody2D = null;

    // Velocity
    private Vector3 velocity = Vector3.zero;

    // Move speed
    [Range(0, 0.3f)]
    public float moveSpeedFactor = 0.0f;

    // Input
    [Range(-1.0f, 1.0f)]
    public float horizontalInput = 0f;

    void Update()
    {
        // Input for horizontal movement
        this.horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        // Handle horizontal movement
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector3(this.horizontalInput * 10f, this.Rigidbody2D.velocity.y, 0f);
            // And then smoothing it out and applying it to the character
            this.Rigidbody2D.velocity = Vector3.SmoothDamp(this.Rigidbody2D.velocity, targetVelocity, ref velocity, this.moveSpeedFactor);
        }

        // Handle flip
        this.HandleFlip();
    }

    // Flip
    private bool facingRight = true;

    private void HandleFlip()
    {
        // If the input is moving the player right and the player is facing left...
        if (this.horizontalInput > 0 && facingRight == false)
        {
            // Flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (this.horizontalInput < 0 && facingRight == true)
        {
            // Flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 invertedScale = transform.localScale;
        invertedScale.x *= -1;

        // Apply
        transform.localScale = invertedScale;
    }
}
