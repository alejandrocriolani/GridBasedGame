using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum MouseState { NONE, MOVE, PUT, ERASE };

    [SerializeField]
    private GameObject objectToCreate;

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
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            ray = Camera.main.ScreenPointToRay(mousePosition);

            switch (mouseState)
            {
                case MouseState.PUT:
                    bool freeSpace = true;
                    if (objectToCreate == null)
                        return;

                    //Vector3 objectSize = objectToCreate.GetComponent<Renderer>().bounds.size;

                    Vector3 objectSize = objectToCreate.GetComponent<ObjectProperties>().size;
                    objectSize = objectSize * 0.5f;

                    if (Physics.Raycast(ray, out hit))
                    {
                        RaycastHit[] hitElements = Physics.BoxCastAll(hit.transform.position, objectSize, Vector3.down);

                        Debug.Log(hitElements.Length);

                        foreach (RaycastHit hitElem in hitElements)
                        {
                            Transform objectHit = hitElem.transform;
                            SpriteRenderer sprite = objectHit.GetComponentInChildren<SpriteRenderer>();
                            if (sprite.color == Color.gray)
                            {
                                Debug.Log("No Free Space");
                                mouseState = MouseState.MOVE;
                                freeSpace = false;
                                break;
                            }
                        }

                        if (freeSpace == false)
                            break;

                        foreach (RaycastHit hitElem in hitElements)
                        {
                            Transform objectHit = hitElem.transform;
                            SpriteRenderer sprite = objectHit.GetComponentInChildren<SpriteRenderer>();
                            sprite.color = Color.grey;
                        }

                        Debug.Log("Instanciate!");
                        Debug.Log(objectToCreate.name);
                        Vector3 objectPosition = hit.transform.position;
                        //objectPosition.z -= objectToCreate.transform.localScale.z / 2;
                        Instantiate(objectToCreate, objectPosition, objectToCreate.transform.rotation, hit.transform);
                    }

                    mouseState = MouseState.MOVE;

                    break;
                case MouseState.NONE:
                case MouseState.MOVE:
                default:
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log("Hay algo " + hit.transform.name);
                        Vector3 direction = Vector3.up * 10;
                        Debug.DrawRay(hit.transform.position, direction, Color.blue);
                        //Transform objetHit = hit.transform;
                        //SpriteRenderer sprite = objetHit.GetComponentInChildren<SpriteRenderer>();
                        //sprite.color = Color.grey;
                    }
                    else
                    {
                        Debug.Log("Aca no hay nada");
                    }  
                    break;
            }
        }

        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Click Derecho!");
            if (objectToCreate != null)
            {
                Debug.Log("Rotate");
                objectToCreate.transform.Rotate(Vector3.up, 90);
                Vector3 objectSize = objectToCreate.GetComponent<ObjectProperties>().size;
                Quaternion rotation = Quaternion.AngleAxis(90, Vector3.up);
                objectSize = rotation * objectSize;

                if (objectSize.x < 0)
                    objectSize.x = -objectSize.x;
                if (objectSize.y < 0)
                    objectSize.y = -objectSize.y;
                if (objectSize.z < 0)
                    objectSize.z = -objectSize.z;

                objectToCreate.GetComponent<ObjectProperties>().size = objectSize;
                Debug.Log(objectSize);
            }
        }
    }

    public void PutElement(GameObject gameObject)
    {
        objectToCreate = gameObject;
        mouseState = MouseState.PUT;
        Debug.Log("Place objects");
    }
}

