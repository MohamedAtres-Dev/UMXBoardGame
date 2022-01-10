using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutTile : Tile
{
    int numOfMovingTiles;

    private void OnEnable()
    {
        if ((currentTilePosition.y + 1) < GameManager.Instance.numOfRows)
        {
            numOfMovingTiles = Random.Range(1, 2);
        }
        else
        {
            numOfMovingTiles = 1;
        }
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);      
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerMovement>();
            
            //Move the Player to the lead tile 
            for (int i = 0; i < numOfMovingTiles; i++)
            {
                player.MoveMechanic(new Vector2(0, 1));
            }
        }
    }
}
