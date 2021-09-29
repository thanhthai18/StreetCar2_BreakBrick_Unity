using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_StreetCarMinigame2 : MonoBehaviour
{
    public WaypointCheck_StreetCarMinigame2 currentWaypoint;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PointWindow"))
        {
            currentWaypoint = collision.GetComponent<WaypointCheck_StreetCarMinigame2>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PointWindow"))
        {
            collision.GetComponent<WaypointCheck_StreetCarMinigame2>().colorIndex = -1;
            currentWaypoint = null;
        }
    }
}
