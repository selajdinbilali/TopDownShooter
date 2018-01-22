using UnityEngine;
using System.Collections;
using System.Collections.Generic;
////////////////////////////////////////////////////////////////////////////////
public class MapGenerator : MonoBehaviour
{

    
    public Transform tilePrefab;
    // we store the mapsize in a vector 2
    public Transform obstaclePrefab;
    public Vector2 mapSize;
    // defines a range in the editor
    [Range(0,1)]
    public float outlinePercent;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;

    public int seed = 10;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        // we create an array (list)
        allTileCoords = new List<Coord>();
        // we instatiate every cell
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                // add the Coords in the list
                allTileCoords.Add(new Coord(x, y));
            }
        }
        // we shuffle the array
        shuffledTileCoords = new Queue<Coord>(
            Utility.ShuffleArray(allTileCoords.ToArray(),seed));

        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            // detroy immediate because we call it from the editor
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab,
                    tilePosition,Quaternion.Euler(Vector3.right * 90)) 
                    as Transform;

                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
            }
        }

        // we decide that there is 10 obstacles
        int obstacleCount = 10;
        // we loop over them
        for (int i = 0; i < obstacleCount; i++)
        {
            // we get a random coordinate
            Coord randomCoord = GetRandomCoord();
            // we convert the coordinates in the area to real world position points
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x,
                randomCoord.y);
            // we instantiate the prefab
            Transform newObstacle = Instantiate(obstaclePrefab,
                obstaclePosition + Vector3.up * 0.5f,
                Quaternion.identity) as Transform;

            newObstacle.parent = mapHolder;
        }
    }

    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x,
            0,
            -mapSize.y / 2 + 0.5f + y);
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int ux, int uy)
        {
            x = ux;
            y = uy;
        }
    }

	
}
