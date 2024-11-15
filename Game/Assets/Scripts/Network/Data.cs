using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Data
{
    public int id;
    public float score;
    public string playerName;

    public Data(int id, float score, string playerName)
    {
        this.id = id;
        this.score = score;
        this.playerName = playerName;
    }

    byte[] Serialize()
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(id);
                writer.Write(score);
                writer.Write(playerName);
            }
            return stream.ToArray(); // Devolvemos el array de bytes
        }
    }

    Data Deserialize(byte[] byteArray)
    {
        using (MemoryStream stream = new MemoryStream(byteArray))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int id = reader.ReadInt32();
                float score = reader.ReadSingle();
                string playerName = reader.ReadString();

                return new Data(id, score, playerName);
            }
        }
    }
}
