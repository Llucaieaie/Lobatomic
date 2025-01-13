using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public enum AttackDirection
{
    NONE = 0,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

[System.Serializable]
public struct TilePosition
{
    public int x;
    public int y;
    public int z;

    public TilePosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Int GetPos()
    {
        return new Vector3Int(x, y, z);
    }
}

[System.Serializable]
public class PlayerData
{
    public int Id = 0;
    public string Name = "No Name";
    public Vector3 Position = Vector3.zero;
    public AttackDirection attackDirection;

    // LISTA DE TILES DESTRUIDAS
    public List<TilePosition> destroyedTilePos = new List<TilePosition>();

    public PlayerData() { Id = 0; Name = "No Name"; Position = Vector3.zero; }

    // Serialize PlayerData to XML
    public static byte[] Serialize(PlayerData data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
        MemoryStream stream = new MemoryStream();

        serializer.Serialize(stream, data);
        byte[] dataBuffer = stream.ToArray();

        //Debug.Log("Serialized XML: " + System.Text.Encoding.UTF8.GetString(dataBuffer));

        return dataBuffer;
    }

    // Deserialize XML to PlayerData
    public static PlayerData Deserialize(byte[] bytes)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
        MemoryStream stream = new MemoryStream(bytes);
        
        stream.Write(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);

        //string receivedXml = System.Text.Encoding.UTF8.GetString(bytes);
        //Debug.Log("Received XML: " + receivedXml);

        return (PlayerData)serializer.Deserialize(stream);
    }
}