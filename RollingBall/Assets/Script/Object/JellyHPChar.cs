using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyHPChar : MonoBehaviour
{
    public Animator Anim = null;

    void Awake()
    {
        Anim.SetTrigger("Idle");
    }
}
