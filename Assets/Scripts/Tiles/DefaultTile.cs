using UnityEngine;


/// <summary>
/// this is the default tile script if there is any logic on this tile i can do.
/// </summary>
public class DefaultTile : Tile
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
