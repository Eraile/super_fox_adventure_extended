using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Try to find a player with an inventory attached
        FoxCharacterHealth foxCharacterHealth = other.GetComponentInParent<FoxCharacterHealth>();
        if (foxCharacterHealth != null)
        {
            // Attribute key to inventory
            foxCharacterHealth.Damage(1);

            // Push fox
            FoxCharacterController foxCharacterController = other.GetComponentInParent<FoxCharacterController>();
            if (foxCharacterController != null)
            {
                // Is the fox left or right of the damage zone center
                bool foxIsOnLeft = (foxCharacterController.transform.position.x < this.transform.position.x);
                if (foxIsOnLeft == true)
                {
                    foxCharacterController.transform.position += (new Vector3(-2f, 1.5f));
                }
                else
                {
                    foxCharacterController.transform.position += (new Vector3(2f, 1.5f));
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
}
