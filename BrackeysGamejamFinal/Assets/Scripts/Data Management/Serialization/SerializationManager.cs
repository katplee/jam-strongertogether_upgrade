using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager
{
    public static bool Save(string subFileName, string saveName, object saveData, out string pathCopy)
        //in this case, the subFileName would be the level of the game you are in
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        //code to create the saves directory
        if (!Directory.Exists(Application.persistentDataPath + $"/saves/{subFileName}"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + $"/saves/{subFileName}");
        }

        //create the path in which a stream will be opened
        string path = Application.persistentDataPath + $"/saves/{subFileName}/" + saveName + ".save";
        //create a copy of the path generated to be stored in the class whose info were serialized
        pathCopy = path;

        //open the stream using the path which was just created
        FileStream stream = File.Create(path);

        //with the stream created, data can now be serialized
        //serialization is done with the formatter
        formatter.Serialize(stream, saveData);

        //always close the file stream after serialization
        stream.Close();

        //to indicate a successful save
        Debug.Log("Save done!");
        return true;
    }

    public static object Load(string path)
    {
        //checks if the path is empty
        if (!File.Exists(path)) { return null; }

        //do the requirements to deserialize with a formatter
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream stream = File.Open(path, FileMode.Open);

        try
        {
            object data = formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            stream.Close();
            return null;
        }        
    }

    //this is where the serialization surrogates will be set
    private static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        SurrogateSelector selector = new SurrogateSelector();

        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }
}
