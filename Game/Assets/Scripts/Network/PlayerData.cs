using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Id = 0;
    public string Name = "No Name";
    public Vector3 Position = Vector3.zero;

    public PlayerData() { Id = 0; Name = "No Name"; Position = Vector3.zero; }

    // Serialize PlayerData to XML
    public static byte[] Serialize(PlayerData data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.Serialize(stream, data);
            return stream.ToArray();
        }
    }

    // Deserialize XML to PlayerData
    public static PlayerData Deserialize(byte[] bytes)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            return (PlayerData)serializer.Deserialize(stream);
        }
    }
}
