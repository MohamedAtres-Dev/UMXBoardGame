using System.Collections;
using UnityEngine;

public class ShortcutTile : Tile
{
    int numOfMovingTiles;

    private void OnEnable()
    {
        if ((currentTilePosition.y + 1) < GameManager.Instance.numOfRows)
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


            if (player.currentState == PlayerMovement.PlayerState.STOPPING)
            {
                StartCoroutine(MovePlayer(player));            
            }

        }
    }

    IEnumerator MovePlayer(PlayerMovement player)
    {
        //Move the Player to the lead tile 
        for (int i = 0; i < numOfMovingTiles; i++)
        {    
            player.MoveMechanic(new Vector2(0, 1));
            yield return new WaitForSeconds(0.2f);
        }
    }
}
