using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public Vector2 currentTilePos;
    public Tile currentTile;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Tile>() != null)
        {
            currentTile = collision.gameObject.GetComponent<Tile>();
            currentTilePos = currentTile.currentTilePosition;
            Debug.Log("Tile Position " + currentTilePos);
        }
    }
}
