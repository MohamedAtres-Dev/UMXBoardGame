using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTile : Tile
{

    
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //StopCoroutine(playersIndex[currentPlayerCount - 1].movePlayer);
        

        if (currentPlayerCount == 3)
        {
            Debug.Log("Finish the Game and send index List to GAme MAnager");
            GameManager.Instance.ShowResultPanel(playersIndex);
        }
    }

    
}
