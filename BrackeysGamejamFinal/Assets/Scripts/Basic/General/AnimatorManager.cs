using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public static Animator Animator { get; private set; }

    private void Start()
    {
        Animator = GetComponent<Animator>();   
    }
}
