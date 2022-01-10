using UnityEngine;

public class PitfallTile : Tile
{
    int numOfMovingTiles;

    private void OnEnable()
    {
        if(currentTilePosition.y > 2)
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
                player.MoveMechanic(new Vector2(0 , -1));
            }
             
        }
    }
}
