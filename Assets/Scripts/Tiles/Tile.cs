using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Tile : MonoBehaviour
{
    public Vector2 currentTilePosition;
    protected int currentPlayerCount = 0;

    

    protected List<PlayerMovement> playersIndex = new List<PlayerMovement>();

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Put here the base logic of the tiles
        if (collision.gameObject.CompareTag("Player"))
        {
            currentPlayerCount++;
            playersIndex.Add(collision.gameObject.GetComponent<PlayerMovement>());
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentPlayerCount--;
            playersIndex.Remove(collision.gameObject.GetComponent<PlayerMovement>());
        }
    }
}
