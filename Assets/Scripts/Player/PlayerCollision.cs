using UnityEngine;


/// <summary>
/// This script is responsible for all player collisions with other objects
/// </summary>
public class PlayerCollision : MonoBehaviour
{

    [HideInInspector] public Vector2 currentTilePos; //Get the current position of the tile that the player is staying
    [HideInInspector] public Tile currentTile;  //Get the current tile that the player is staying using this to change the parent of the player

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Tile>() != null)
        {
            currentTile = collision.gameObject.GetComponent<Tile>();
            currentTilePos = currentTile.currentTilePosition;        
        }
    }
}
