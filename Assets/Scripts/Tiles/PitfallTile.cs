using UnityEngine;
using System.Collections;

public class PitfallTile : Tile
{
    int numOfMovingTiles;

    private void OnEnable()
    {
        if(currentTilePosition.y > 2)
        {
            numOfMovingTiles = Random.Range(1, 3);
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
            
            if(player.currentState == PlayerMovement.PlayerState.STOPPING)
            {
                //Move the Player to the lead tile 
                StartCoroutine(MovePlayer(player));
            }

             
        }
    }

    IEnumerator MovePlayer(PlayerMovement player)
    {
        for (int i = 0; i < numOfMovingTiles; i++)
        {
            player.MoveMechanic(new Vector2(0, -1));
            yield return new WaitForSeconds(0.2f);
        }
    }

}
