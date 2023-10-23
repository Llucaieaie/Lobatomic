using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public enum TileType
{
    HAPPY=0,
    INMUNE,
    NORMAL,
    POWER_UP,
    SAD,
    VOID,
    EXPLOSIVE,
}

[System.Serializable]
public struct TileStruct
{
    public GameObject tile;
    public TileType tileType;
    public int maxNum, tileCount, appearRate;
};

public class MapGenerator : MonoBehaviour
{
    [SerializeField] TileStruct[] tileStruct;
    public Tilemap tileMap;
    public TileBase tileBase;


    public List<GameObject> tiles = new List<GameObject>();
    List<Vector3Int> occupied = new List<Vector3Int>();
    HashSet<Vector3Int> positionsFromTileFrame = new HashSet<Vector3Int>();
    BoundsInt bounds;
    int sizeX, sizeY;

    private void Start()
    {
        GenerateMap(50, 80);
    }

    public void GenerateMap(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        bounds = new BoundsInt(new Vector3Int(0,0,0) - new Vector3Int(sizeX/2, sizeY/2, 0),
                                            new Vector3Int(sizeX, sizeY, 0));

        SaveZone();

        positionsFromTileFrame = CreateRoom(bounds);

        for (int i = 0; i < tileStruct.Length; i++)
        {
            tileStruct[i].maxNum = (tileStruct[i].maxNum*(sizeX*sizeY))/100;
        }

        foreach (var position in positionsFromTileFrame)
        {
            for (int i = 0; i < tileStruct.Length; i++)
            {
                if (!IsTiledOcccupied(position) && IsInRate(tileStruct[i]) && tileStruct[i].maxNum > tileStruct[i].tileCount)
                {
                    PaintTiles(position, tileStruct[i]);
                    tileStruct[i].tileCount++;
                }
            }
        }
    }

    private void SaveZone()
    {
        occupied.Add(new Vector3Int(0, 0, 0));
        occupied.Add(new Vector3Int(1, 0, 0));
        occupied.Add(new Vector3Int(-1, 0, 0));
        occupied.Add(new Vector3Int(0, 1, 0));
        occupied.Add(new Vector3Int(0, -1, 0));
        occupied.Add(new Vector3Int(1, -1, 0));
        occupied.Add(new Vector3Int(-1, -1, 0));
        occupied.Add(new Vector3Int(1, 1, 0));
        occupied.Add(new Vector3Int(-1, 1, 0));

        occupied.Add(new Vector3Int(2, 0, 0));
        occupied.Add(new Vector3Int(-2, 0, 0));
        occupied.Add(new Vector3Int(0, 2, 0));
        occupied.Add(new Vector3Int(0, -2, 0));
    }

    private HashSet<Vector3Int> CreateRoom(BoundsInt room)
    {
        HashSet<Vector3Int> floor = new HashSet<Vector3Int>();

        for (int col = 0; col < room.size.x; col++)
        {
            for (int row = 0; row < room.size.y; row++)
            {
                Vector3Int position = room.min + new Vector3Int(col, row);
                floor.Add(position);
            }
        }
        return floor;
    }

    public void PaintTiles(Vector3Int position, TileStruct tile)
    {
        occupied.Add(position);
        tiles.Add(Instantiate(tile.tile, new Vector3(position.x, position.y, 0), Quaternion.Euler(180, 0, 0)));
    }

    private bool IsInRate(TileStruct tile)
    {
        bool ret = false;

        if (Random.Range(0,101) <= (tile.appearRate + (tile.maxNum/(tile.maxNum-tile.tileCount+1))/10))
        {
            ret = true;
        }

        return ret;
    }

    bool IsTiledOcccupied(Vector3Int position)
    {
        bool ret = false;

        foreach (var posOccupied in occupied)
        {
            if (position == posOccupied)
            {
                ret = true;
            }
        }
        return ret;
    }

    void RestartLvl()
    {
        occupied.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].SendMessage("OnExplosion");
        }
        tiles.Clear();

        GenerateMap(sizeX, sizeY);
    }

    void CleanUp()
    {
        occupied.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].SendMessage("OnExplosion");
        }
        tiles.Clear();
    }
}