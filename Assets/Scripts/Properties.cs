using System;
using UnityEngine;

public class Properties : MonoBehaviour
{


    [field: SerializeField] public float acceleration {get; private set;} = 1.0f;
    [field: SerializeField] public float max_velocity {get; private set;} = 10.0f;



    void Start()
    {

    }

    void FixedUpdate()
    {
    }
}
