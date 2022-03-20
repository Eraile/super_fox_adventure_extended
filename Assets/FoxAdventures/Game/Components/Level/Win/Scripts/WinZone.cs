using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WinZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Try to find a player with an inventory attached
        FoxPlayer foxPlayer = other.GetComponentInParent<FoxPlayer>();
        if (foxPlayer != null)
        {
            foxPlayer.Win();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
}
