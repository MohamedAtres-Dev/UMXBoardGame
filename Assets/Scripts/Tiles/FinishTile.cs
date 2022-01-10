using UnityEngine;


/// <summary>
/// THis tile is the finish tile which is responsible for sending wining players to game manager 
/// </summary>
public class FinishTile : Tile
{

    public GameObject winEffect;


    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);      
       
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //change the current state of the player to finished so we can now skip his turn
            collision.gameObject.GetComponent<PlayerMovement>().currentState = PlayerMovement.PlayerState.FINISHED;

            //Play Wining sound
            audioManager.PlaySound(playableSound);

            //Play wining Effect
            Instantiate(winEffect, transform.position, Quaternion.identity);
        }

        if (currentPlayerCount == 3)
        {
            //Finish the Game and send index List to GAme MAnager
            GameManager.Instance.ShowResultPanel(playersIndex);
        }
    }
}
