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
    [Range(0, 1)]
    public float outlinePercent;

    [Range(0, 1)]
    public float obstaclePercent;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;

    public int seed = 10;

    Coord mapCentre;

    void Start()
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

        mapCentre = new Coord( (int)mapSize.x / 2, (int)mapSize.y / 2);

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



        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        // we decide that there is 10 obstacles
        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);

        int currentObstacleCount = 0;
        // we loop over them
        for (int i = 0; i < obstacleCount; i++)
        {
            // we get a random coordinate
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                

                // we convert the coordinates in the area to real world position points
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x,
                    randomCoord.y);



                // we instantiate the prefab
                Transform newObstacle = Instantiate(obstaclePrefab,
                    obstaclePosition + Vector3.up * 0.5f,
                    Quaternion.identity) as Transform;


                newObstacle.parent = mapHolder;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }

 

        }
    }


    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCentre);
        mapFlags[mapCentre.x, mapCentre.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();

            // loop the 4 adjacent
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                // find a new and add a queue
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
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

        //equals and not equals operators
        public static bool operator ==(Coord c1, Coord c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
    }

	
}
