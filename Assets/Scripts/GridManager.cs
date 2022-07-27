using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public GameObject player;
    public Tilemap groundTilemap;
    public Tilemap uiOverlayTilemap;
    public Tilemap collidersTilemap;

    public float tileSize = 1f;
    public float xTilePivot = 0.5f;
    public float yTilePivot = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
