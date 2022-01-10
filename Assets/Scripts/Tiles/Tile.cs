using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THis is a parent class for all tiles 
/// contains the general logic for all tiles 
/// </summary>
public abstract class Tile : MonoBehaviour
{
    [Tooltip("THis is USed for Player Movement also Indexing each tile")]
    public Vector2 currentTilePosition;

    [Tooltip("THis is USed for Players count on each tile")]
    protected int currentPlayerCount = 0;

    [Tooltip("THis is USed to Play audios")]
    public AudioManager audioManager;

    [Tooltip("This is the sound to play on each special tile")]
    public AudioClip playableSound;

    //Make a list for all players movement component on each tile this is very useful for finish tile to set winners conditions
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
