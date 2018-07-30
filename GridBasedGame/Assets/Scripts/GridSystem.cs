using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    public GameObject defaultTile;
    public Color grassColor;

    public uint width = 1;
    public uint height = 1;

    private GameObject[,] gridMap;

    // Use this for initialization
    void Start()
    {
        GenerateTerrain();
        //GenerateLakes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Destroy(gridMap[i, j]);
            }
        }

    }


    private void GenerateTerrain()
    {
        SpriteRenderer tileRenderer = defaultTile.GetComponentInChildren<SpriteRenderer>();
        tileRenderer.color = grassColor;

        float startPosX = -(width / 2);
        float startPosY = -(height / 2);

        gridMap = new GameObject[width, height];

        Vector3 instPosition = Vector3.zero;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                instPosition.x = startPosX + x;
                instPosition.z = startPosY + z;
                gridMap[x, z] = Instantiate(defaultTile, instPosition, 
                    Quaternion.AngleAxis(-90, Vector3.right));
            }
        }
    }

    private void GenerateRiver()
    {

    }

    private void GenerateLakes()
    {
        int lakesCant = Random.Range(2, 10);
        int xPos = 0;
        int yPos = 0;
        int lakeWidth = 2;
        int lakeHeight = 2;

        Debug.Log("Lakes to creates: " + lakesCant);

        for (int i = 0; i < lakesCant; i++)
        {
            xPos = Random.Range(0, (int)width + 1);
            yPos = Random.Range(0, (int)height + 1);
            lakeWidth = Random.Range(2, 16);
            lakeHeight = Random.Range(2, 16);

            Debug.Log("Lake (" + i + "): width = " + lakeWidth + ", heigth = " + lakeHeight + ", xPos = " + xPos + ", yPos = " + yPos);

            for (int j = 0; j < lakeWidth; j++)
            {
                for (int k = 0; k < lakeHeight; k++)
                {
                    if ((xPos + j < width) && (yPos + k < height))
                    {
                        SpriteRenderer spriteRenderer = gridMap[j, k].GetComponentInChildren<SpriteRenderer>();
                        spriteRenderer.color = Color.blue;
                    }
                    else
                        break;
                }
            }
        }
    }
}
