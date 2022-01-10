using System.Collections;
using UnityEngine;


/// <summary>
/// THis script is for shortcut tile which is a special tile 
/// </summary>
public class ShortcutTile : Tile
{

    //the number of moving tiles that which this tile will contain to move the player
    int numOfMovingTiles;

   
    private void OnEnable()
    {
        //Set the number depends on the position of this tile 
        if ((currentTilePosition.y + 2) < GameManager.Instance.numOfRows)
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

        //Move the Player for Lead tile 
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerMovement>();


            if (player.currentState == PlayerMovement.PlayerState.STOPPING)
            {
                //Play Special sound
                audioManager.PlaySound(playableSound);

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
