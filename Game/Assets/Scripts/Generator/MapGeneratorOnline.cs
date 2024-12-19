using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class MapGeneratorOnline : MonoBehaviour
{
    // Serialized fields
    [SerializeField] TileStruct[] tileStruct;
    public Tilemap tileMap;
    public OnlineGameManager onlineGameManager;
    public List<GameObject> tiles = new List<GameObject>();
    [SerializeField] int sizeX, sizeY;
    public GameObject[] groundTiles;
    public int padding;

    public GameObject GroundTilesParent;
    public GameObject TilesParent;

    [Space]
    [SerializeField] private int seed = 0;

    // Not serialized fields
    HashSet<Vector3Int> positionsFromTileFrame = new HashSet<Vector3Int>();
    List<Vector3Int> occupied = new List<Vector3Int>();
    BoundsInt bounds;

    private int auxTileIDIterator = 0;

    private void Start()
    {
        for (int i = 0; i < tileStruct.Length; i++)
        {
            tileStruct[i].maxNumStatic = tileStruct[i].maxNum;
        }

        GameObject ogm = GameObject.Find("Online Game Manager");
        if (ogm != null) onlineGameManager = ogm.GetComponent<OnlineGameManager>();

        GenerateMap(sizeX, sizeX);
    }

    public void GenerateMap(int sizeX, int sizeY)
    {
        if (seed == 0)
        {
            seed = Random.Range(1, int.MaxValue);
            Debug.Log("Generated Seed: " + seed);
        }
        Random.InitState(seed);

        bounds = new BoundsInt(new Vector3Int(0, 0, 0) - new Vector3Int(sizeX / 2, sizeY / 2, 0),
                                            new Vector3Int(sizeX, sizeY, 0));

        SaveZone();
        GenerateExternalCollider();

        positionsFromTileFrame = CreateRoom(bounds);

        for (int i = 0; i < tileStruct.Length; i++)
        {
            tileStruct[i].tileCount = 0;
            tileStruct[i].maxNum = tileStruct[i].maxNumStatic;
            tileStruct[i].maxNum = (tileStruct[i].maxNumStatic * (sizeX * sizeY)) / 100;
        }

        // Create tiles
        foreach (var position in positionsFromTileFrame)
        {
            for (int i = 0; i < tileStruct.Length; i++)
            {
                PaintGroundTiles(position);

                if (position.x < bounds.xMin || position.x >= bounds.xMax || position.y < bounds.yMin || position.y >= bounds.yMax)
                {
                    occupied.Add(position);
                    GameObject newTile = Instantiate(tileStruct[0].tile, new Vector3(position.x, position.y, 0), Quaternion.identity);
                    newTile.transform.parent = TilesParent.transform;
                    newTile.transform.SetAsLastSibling();

                    tiles.Add(newTile);
                }
                else if (!IsTiledOcccupied(position) && IsInRate(tileStruct[i]) && tileStruct[i].maxNum > tileStruct[i].tileCount)
                {
                    PaintTiles(position, tileStruct[i], auxTileIDIterator);
                    auxTileIDIterator++;
                    tileStruct[i].tileCount++;
                }
            }
        }

        onlineGameManager.currentTiles = new List<GameObject>(tiles);
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

        for (int col = -padding; col < room.size.x + padding; col++)
        {
            for (int row = -padding; row < room.size.y + padding; row++)
            {
                Vector3Int position = room.min + new Vector3Int(col, row);
                floor.Add(position);
            }
        }
        return floor;
    }

    private void PaintGroundTiles(Vector3Int position)
    {
        int rand = Random.Range(0, 2);
        GameObject tile = Instantiate(groundTiles[rand], new Vector3(position.x, position.y, 1), Quaternion.identity);
        tile.transform.parent = GroundTilesParent.transform;
        tile.transform.SetAsLastSibling();
    }

    // Create tile and set ID
    public void PaintTiles(Vector3Int position, TileStruct tile, int newID)
    {
        occupied.Add(position);

        GameObject newTile = Instantiate(tile.tile, new Vector3(position.x, position.y, 0), Quaternion.identity);
        newTile.GetComponent<Tile>().tileID = newID;
        newTile.transform.parent = TilesParent.transform;
        newTile.transform.SetAsLastSibling();

        tiles.Add(newTile);
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
                Destroy(tiles[i].gameObject);
            }
        }
        tiles.Clear();

        yield return new WaitForSeconds(0.5f);

        GenerateMap(sizeX, sizeY);
    }
    public IEnumerator CleanUp()
    {
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
