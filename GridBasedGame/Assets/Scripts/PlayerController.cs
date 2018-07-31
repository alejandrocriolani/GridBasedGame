using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum MouseState { NONE, MOVE, PUT, ERASE };

    public Color avalibleColor;
    public Color notAvalibleColor;

    [SerializeField]
    private GameObject objectToCreate;
    Color originalObjectColor;
    Vector3 objectSize;

    private MouseState mouseState;
    private RaycastHit hit;
    private Ray ray;

    HashSet<BuildingInfo> buildingInfoSet;

    // Use this for initialization
    void Start()
    {
        buildingInfoSet = new HashSet<BuildingInfo>();
        mouseState = MouseState.MOVE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(mouseState)
        {
            case MouseState.ERASE:
                EraseBuilding();
                break;
            case MouseState.MOVE:
            case MouseState.NONE:
                break;
            case MouseState.PUT:
                CreateBuilding();
                break;
            default:
                break;
        }
    }

    private void EnableColliders(bool value)
    {
        foreach (BuildingInfo info in buildingInfoSet)
        {
            if(info != null)
            {
                info.EnableColliders(value);
            }
        }
    }

    private void CreateBuilding()
    {
        if(objectToCreate == null)
        {
            mouseState = MouseState.MOVE;
            return;
        }

        EnableColliders(false);

        Vector3 mousePosition = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            objectToCreate.transform.position = hit.transform.position;
            RaycastHit[] hitElements = Physics.BoxCastAll(hit.transform.position,
                objectSize * 0.5f, Vector3.down);

            if(FreeSpace(hitElements) && Input.GetMouseButtonDown(0))
            {
                List<GameTile> gameTiles = new List<GameTile>();
                foreach(RaycastHit elem in hitElements)
                {
                    GameTile gameTile = elem.transform.GetComponent<GameTile>();
                    if(gameTile != null)
                    {
                        gameTiles.Add(gameTile);
                    }
                    else
                    {
                        Debug.LogWarning("Game tile is null");
                    }
                }

                objectToCreate.transform.position = hit.transform.position;
                BoxCollider collider = objectToCreate.GetComponent<BoxCollider>();
                if(collider != null)
                {
                    BuildingInfo buildingInfo = 
                        new BuildingInfo(collider, gameTiles, objectToCreate);
                    buildingInfoSet.Add(buildingInfo);
                }
                EnableColliders(true);
                mouseState = MouseState.MOVE;
                objectToCreate = null;
                foreach(RaycastHit tile in hitElements)
                {
                    GameTile tileInfo = tile.transform.GetComponent<GameTile>();
                    if(tileInfo != null)
                    {
                        tileInfo.SetBuilding(GroundType.BUILD);
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Destroy(objectToCreate);
                objectToCreate = null;
                mouseState = MouseState.MOVE;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Rotate(-90);
                Debug.Log("Rotate CounterClockwise");
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Rotate ClockWise");
                Rotate(90);
            }
        }
    }

    private bool FreeSpace(RaycastHit[] hitElements)
    {
        foreach (RaycastHit element in hitElements)
        {
            GameTile tileInfo = element.transform.GetComponent<GameTile>();
            if (tileInfo != null)
            {
                if (!tileInfo.Free)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void Rotate(float angle)
    {
        if (objectToCreate != null)
        {
            Vector3 objectSize = objectToCreate.GetComponent<ObjectProperties>().size;
            ObjectProperties.AxisRotation axisRotation = objectToCreate.GetComponent<ObjectProperties>().axisRotation;
            float aux;
            switch (axisRotation)
            {
                case ObjectProperties.AxisRotation.X:
                    if(objectSize.x != objectSize.z)
                    {
                        aux = objectSize.x;
                        objectSize.x = objectSize.z;
                        objectSize.z = aux;
                    }
                    objectToCreate.transform.Rotate(Vector3.right, angle);
                    break;
                case ObjectProperties.AxisRotation.Y:
                    if (objectSize.x != objectSize.z)
                    {
                        aux = objectSize.x;
                        objectSize.x = objectSize.z;
                        objectSize.z = aux;
                    }
                    objectToCreate.transform.Rotate(Vector3.up, angle);
                    break;
                case ObjectProperties.AxisRotation.Z:
                    if (objectSize.x != objectSize.z)
                    {
                        aux = objectSize.x;
                        objectSize.x = objectSize.z;
                        objectSize.z = aux;
                    }
                    objectToCreate.transform.Rotate(Vector3.forward, angle);
                    break;
                default:
                    break;
            }
            

            objectSize.x = Mathf.Abs(objectSize.x);
            objectSize.y = Mathf.Abs(objectSize.y);
            objectSize.z = Mathf.Abs(objectSize.z);

            objectToCreate.GetComponent<ObjectProperties>().size = objectSize;
            Debug.Log(objectSize);
        }
    }

    private void EraseBuilding()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.tag == "Building")
                {
                    foreach(BuildingInfo info in buildingInfoSet)
                    {
                        if(info.gameObject.name == hit.transform.name)
                        {
                            string gameObjectTag = info.gameObject.tag;
                            info.FreeTileSpace();
                            Destroy(info.gameObject);
                            buildingInfoSet.Remove(info);
                            Rename(gameObjectTag);
                            break;
                        }
                    }
                    mouseState = MouseState.MOVE;
                }
            }
        }
    }

    private void Rename(string tag)
    {
        int count = 0;
        foreach(BuildingInfo info in buildingInfoSet)
        {
            if(info.gameObject.tag == tag)
            {
                info.gameObject.name = tag + count;
                count++;
            }
        }
    }

    public void DeleteBuilding()
    {
        mouseState = MouseState.ERASE;
    }

    public void PutElement(GameObject gameObject)
    {
        if(gameObject != null)
        {
            objectToCreate = Instantiate(gameObject);
            objectToCreate.name = objectToCreate.tag + buildingInfoSet.Count;
            objectSize = gameObject.GetComponent<ObjectProperties>().size;
            objectToCreate.GetComponent<BoxCollider>().enabled = false;
            mouseState = MouseState.PUT;
            Debug.Log("Place objects");
        }

    }
}

