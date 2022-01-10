using UnityEngine;
using System.Collections;
/// <summary>
/// THis script is for Pitfall tile which is a special tile
/// </summary>
public class PitfallTile : Tile
{

    //the number of moving tiles that which this tile will contain to move the player
    int numOfMovingTiles;

    private void OnEnable()
    {
        //Set the number depends on the position of this tile 
        if (currentTilePosition.y > 2)
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
            
            if(player.currentState == PlayerMovement.PlayerState.STOPPING)
            {

                //Play Special sound
                audioManager.PlaySound(playableSound);
                             
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
