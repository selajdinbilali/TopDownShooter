using UnityEngine;
using System.Collections;
////////////////////////////////////////////////////////////////////////////////
public class MapGenerator : MonoBehaviour
{

    
    public Transform tilePrefab;
    // we store the mapsize in a vector 2
    public Vector2 mapSize;
    // defines a range in the editor
    [Range(0,1)]
    public float outlinePercent;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {

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
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0,
                    -mapSize.y / 2 + 0.5f + y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, 
                    Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
            }
        }
    }

	
}
