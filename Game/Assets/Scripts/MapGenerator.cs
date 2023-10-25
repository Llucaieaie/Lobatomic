using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;


public enum TileType
{
    HAPPY=0,
    INMUNE,
    NORMAL,
    POWER_UP,
    SAD,
    EXPLOSIVE,
    VOID,
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

    public GameObject Player;
    public GameObject camera;
    public TimerController timerController;
    public List<GameObject> tiles = new List<GameObject>();
    HashSet<Vector3Int> positionsFromTileFrame = new HashSet<Vector3Int>();
    List<Vector3Int> occupied = new List<Vector3Int>();
    BoundsInt bounds;
    [SerializeField] int sizeX, sizeY;

    private void Start()
    {
        GenerateMap(sizeX, sizeX);
    }

    public void GenerateMap(int sizeX, int sizeY)
    {
        bounds = new BoundsInt(new Vector3Int(0, 0, 0) - new Vector3Int(sizeX / 2, sizeY / 2, 0),
                                            new Vector3Int(sizeX, sizeY, 0));

        SaveZone();
        GenerateExternalCollider();

        positionsFromTileFrame = CreateRoom(bounds);

        for (int i = 0; i < tileStruct.Length; i++)
        {
            tileStruct[i].maxNum = (tileStruct[i].maxNum * (sizeX * sizeY)) / 100;
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
        tiles.Add(Instantiate(tile.tile, new Vector3(position.x, position.y, 0), Quaternion.identity));
    }

    private bool IsInRate(TileStruct tile)
    {
        bool ret = false;

        if (Random.Range(0, 101) <= (tile.appearRate + (tile.maxNum / (tile.maxNum - tile.tileCount + 1)) / 10))
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

    void GenerateExternalCollider()
    {
        Vector2[] points = new Vector2[5];

        points[0] = new Vector2(bounds.xMin - 2, bounds.yMax - 1);
        points[1] = new Vector2(bounds.xMax - 2, bounds.yMax - 1);
        points[2] = new Vector2(bounds.xMax - 2, bounds.yMin - 1);
        points[3] = new Vector2(bounds.xMin - 2, bounds.yMin - 1);

        points[4] = points[0];
        GetComponentInParent<EdgeCollider2D>().points = points;
    }

    public IEnumerator RestartLvl()
    {
        occupied.Clear();

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] != null)
            {
                switch (tiles[i].layer)
                {
                    case 6:
                        tiles[i].GetComponent<HappyTile>().OnExplosion();
                        break;
                    case 7:
                        tiles[i].GetComponent<SadTile>().OnExplosion();
                        break;
                    case 8:
                        tiles[i].GetComponent<ExplosiveTile>().OnExplosion();
                        break;
                    case 9:
                        tiles[i].GetComponent<PowerUpTile>().OnExplosion();
                        break;
                    case 10:
                        tiles[i].GetComponent<NormalTile>().OnExplosion();
                        break;
                }
            }
        }
        tiles.Clear();

        Player.transform.position = Vector3.zero;

        timerController.TimeCount += 20;

        yield return new WaitForSeconds(0.5f);

        GenerateMap(sizeX, sizeY);
    }
    public IEnumerator CleanUp()
    {
        camera.GetComponent<CameraManager>().MapDestroy();
        yield return new WaitForSeconds(0.1f);

        occupied.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] != null)
            {
                switch (tiles[i].layer)
                {
                    case 6:
                        tiles[i].GetComponent<HappyTile>().OnExplosion();
                        break;
                    case 7:
                        tiles[i].GetComponent<SadTile>().OnExplosion();
                        break;
                    case 8:
                        tiles[i].GetComponent<ExplosiveTile>().OnExplosion();
                        break;
                    case 9:
                        tiles[i].GetComponent<PowerUpTile>().OnExplosion();
                        break;
                    case 10:
                        tiles[i].GetComponent<NormalTile>().OnExplosion();
                        break;
                }
            }
        }
        tiles.Clear();

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("DeathScene");
    }
}