using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tile
{
    public GameObject tile;
    public float creationTime;

    public Tile(GameObject t, float ct)
    {
        tile = t;
        creationTime = ct;
    }
}

public class GenerateInfinteLandscape : MonoBehaviour
{
    public GameObject plane;
    public GameObject aircraft;

    private int planeSize = 10;
    private int halfTilesX = 25;
    private int halfTilesZ = 25;
    private Vector3 startPos;
    private Hashtable tiles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new Hashtable();
        transform.position = Vector3.zero;
        startPos = Vector3.zero;

        float updateTime = Time.realtimeSinceStartup;

        for (int x = -halfTilesX; x < halfTilesX; x++)
        {
            for (int z = -halfTilesZ; z < halfTilesZ; z++)
            {
                Vector3 pos = new Vector3((x * planeSize + startPos.x), 0, (z * planeSize + startPos.z));
                GameObject t = (GameObject) Instantiate(plane, pos, Quaternion.identity);
                string tileName = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                t.name = tileName;
                Tile tile = new Tile(t, updateTime);
                tiles.Add(tileName, tile);
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        int xMove = (int)(aircraft.transform.position.x - startPos.x);
        int zMove = (int)(aircraft.transform.position.z - startPos.z);


        if (Mathf.Abs(xMove) >= planeSize || Mathf.Abs(zMove) >= planeSize)
        {
        
            float updateTime = Time.realtimeSinceStartup;

            int aircraftX = (int)(Mathf.Floor(aircraft.transform.position.x / planeSize) * planeSize);
            int aircraftZ = (int)(Mathf.Floor(aircraft.transform.position.z / planeSize) * planeSize);

            for (int x = -halfTilesX; x < halfTilesX; x++)
            {
                for (int z = -halfTilesZ; z < halfTilesZ; z++)
                {
                    Vector3 pos = new Vector3((x * planeSize + aircraftX), 0, (z * planeSize + aircraftZ));
                    string tileName = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();

                    if (!tiles.ContainsKey(tileName))
                    {
                        GameObject t = (GameObject) Instantiate(plane, pos, Quaternion.identity);
                        t.name = tileName;
                        Tile tile = new Tile(t, updateTime);
                        tiles.Add(tileName, tile);
                    } else
                    {
                        (tiles[tileName] as Tile).creationTime = updateTime;
                    }
                }
            }

            Hashtable newTerrain = new Hashtable();
            foreach (Tile t in tiles.Values)
            {
                if (t.creationTime != updateTime)
                {
                    Destroy(t.tile);

                } else
                {
                    newTerrain.Add(t.tile.name, t);
                }
            }
            tiles = newTerrain;
            startPos = aircraft.transform.position;
        }
    }



}
