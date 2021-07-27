using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameObjectSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        GameObject gameObject = (GameObject)obj;
        info.AddValue("x", gameObject.transform);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector3 vector3 = (Vector3)obj;
        vector3.x = (float)info.GetValue("x", typeof(float));
        vector3.y = (float)info.GetValue("y", typeof(float));
        vector3.z = (float)info.GetValue("z", typeof(float));
        obj = vector3;
        return obj;
    }
}
