using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messages : MonoBehaviour
{
    public float duration = 1;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
    }
}
