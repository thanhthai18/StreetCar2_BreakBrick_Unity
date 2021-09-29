using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCheck_StreetCarMinigame2 : MonoBehaviour
{
    public int colorIndex = -1;
    public Cube_StreetCarMinigame2 currentCube;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            currentCube = collision.GetComponent<Cube_StreetCarMinigame2>();
            colorIndex = 1;
        }
        if (collision.gameObject.CompareTag("People"))
        {
            currentCube = collision.GetComponent<Cube_StreetCarMinigame2>();
            colorIndex = 2;
        }
        if (collision.gameObject.CompareTag("Balloon"))
        {
            currentCube = collision.GetComponent<Cube_StreetCarMinigame2>();
            colorIndex = 3;
        }
        if (collision.gameObject.CompareTag("Tree"))
        {
            currentCube = collision.GetComponent<Cube_StreetCarMinigame2>();
            colorIndex = 4;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("People") || collision.gameObject.CompareTag("Balloon") || collision.gameObject.CompareTag("Tree"))
        {
            currentCube = null;
            colorIndex = -1;
        }
       
    }
}
