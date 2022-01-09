using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Tile : MonoBehaviour
{
    

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Put here the base logic of the tiles
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        
    }
}
