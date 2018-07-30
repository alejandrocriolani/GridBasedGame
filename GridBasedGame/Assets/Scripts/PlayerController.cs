using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum MouseState { NONE, MOVE, PUT, ERASE };

    [SerializeField]
    private GameObject objectToCreate;
    Vector3 objectSize;

    private MouseState mouseState;
    private RaycastHit hit;
    private Ray ray;

    // Use this for initialization
    void Start()
    {
        mouseState = MouseState.MOVE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(mouseState)
        {
            case MouseState.ERASE:
                break;
            case MouseState.MOVE:
            case MouseState.NONE:
                break;
            case MouseState.PUT:
                CreateElements();
                break;
            default:
                break;
        }
    }

    private void CreateElements()
    {
        if(objectToCreate == null)
        {
            mouseState = MouseState.MOVE;
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            objectToCreate.transform.position = hit.transform.position;
            RaycastHit[] hitElements = Physics.BoxCastAll(hit.transform.position,
                objectSize * 0.5f, Vector3.down);
            
            /*
            foreach(RaycastHit elem in hitElements)
            {
                GameTile gameTile = elem.transform.GetComponent<GameTile>();
                if(gameTile != null)
                {
                    SpriteRenderer sprite = elem.transform.GetComponentInChildren<SpriteRenderer>();
                    if (gameTile.Free)
                    {
                        sprite.color = Color.green;
                    }
                    else
                    {
                        sprite.color = Color.red;
                    }
                }
            }
            */

            if(FreeSpace(hitElements) && Input.GetMouseButtonDown(0))
            {
                objectToCreate.transform.position = hit.transform.position;
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

    private void Rotate(float angle)
    {
        if (objectToCreate != null)
        {
            
            objectToCreate.transform.Rotate(Vector3.up, angle);
            Vector3 objectSize = objectToCreate.GetComponent<ObjectProperties>().size;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            objectSize = rotation * objectSize;

            objectSize.x = Mathf.Abs(objectSize.x);
            objectSize.y = Mathf.Abs(objectSize.y);
            objectSize.z = Mathf.Abs(objectSize.z);

            objectToCreate.GetComponent<ObjectProperties>().size = objectSize;
            Debug.Log(objectSize);
        }
    }

    private bool FreeSpace(RaycastHit[] hitElements)
    {
        foreach(RaycastHit element in hitElements)
        {
            GameTile tileInfo = element.transform.GetComponent<GameTile>();
            if(tileInfo != null)
            {
                if(!tileInfo.Free)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PutElement(GameObject gameObject)
    {
        if(gameObject != null)
        {
            objectToCreate = Instantiate(gameObject);
            objectSize = gameObject.GetComponent<ObjectProperties>().size;
            mouseState = MouseState.PUT;
            Debug.Log("Place objects");
        }

    }
}

