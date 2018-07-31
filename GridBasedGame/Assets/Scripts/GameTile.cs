//Tile Object
using UnityEngine;

public enum GroundType { DIRT, GRASS, WATER, ROAD, BUILD, FARM, TREE, DECORATION, NOT_SETTED };

public class GameTile : MonoBehaviour
{
    private bool free;
    private GroundType groundType;

    void Start()
    {
        free = true;
        groundType = GroundType.NOT_SETTED;
    }

    public bool Free
    {
        get { return free;  }
    }

    public void FreeTile()
    {
        free = true;
        groundType = GroundType.NOT_SETTED;
    }

    public GroundType Type
    {
        get { return groundType;  } 
    }

    public bool SetBuilding(GroundType type)
    {
        if(this.Free)
        {
            free = false;
            groundType = type;
            return true;
        }
        return false;
    }
}
