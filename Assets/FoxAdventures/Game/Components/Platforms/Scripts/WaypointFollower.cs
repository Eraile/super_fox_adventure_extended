using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] private float speed = 2f;

    protected virtual void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            sprite.flipX = !sprite.flipX;
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }

    protected virtual void OnDrawGizmos()
    {
        if (this.waypoints != null && this.waypoints.Length > 0)
        {
            for (int i= 0; i < this.waypoints.Length; i++)
            {
                if (this.waypoints[i] != null && this.waypoints.Length > (i+1) && this.waypoints[i+1] != null)
                {
                    Gizmos.DrawWireSphere(this.waypoints[i].transform.position, 0.5f);
                    Gizmos.DrawLine(this.waypoints[i].transform.position, this.waypoints[i + 1].transform.position);
                    Gizmos.DrawWireSphere(this.waypoints[i + 1].transform.position, 0.5f);
                }
            }
        }
    }
}
