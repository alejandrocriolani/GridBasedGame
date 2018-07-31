using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo
{
    public BoxCollider boxCollider;
    public List<GameTile> gameTiles;
    public GameObject gameObject;

    /*
    public BoxCollider Collider
    {
        set
        {
            if (value != null)
                boxCollider = value;
        }
    }

    
    public List<GameTile> GameTiles
    {
        set
        {
            if (value != null)
            {
                gameTiles = value;
            }
        }
    }*/

    public BuildingInfo()
    {
        boxCollider = null;
        gameTiles = null;
        gameObject = null;
    }
    
    
    public BuildingInfo(BoxCollider collider, List<GameTile> tiles, GameObject gameObj)
    {
        boxCollider = collider;
        gameTiles = tiles;
        gameObject = gameObj;
    }

    public void EnableColliders(bool value)
    {
        if(boxCollider != null)
        {
            boxCollider.enabled = value;
        }
    }

    public void FreeTileSpace()
    {
        foreach(GameTile tile in gameTiles)
        {
            tile.FreeTile();
        }
    }
}
