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
