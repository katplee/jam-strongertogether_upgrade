using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSetter : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetRatio(1024, 768);
    }

    void SetRatio(float w, float h)
    {
        Screen.SetResolution((int)w, (int)h, true);
    }

}
