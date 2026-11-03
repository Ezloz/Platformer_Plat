using System;
using UnityEngine;

public class Properties : MonoBehaviour
{
    public float gravity_scale => this.gameObject.GetComponent<Rigidbody2D>().gravityScale;

    [field: SerializeField] public float acceleration {get; private set;} = 1.0f;
    [field: SerializeField] public float air_acceleration {get; private set;} = 1.0f;

    [field: SerializeField] public float max_velocity {get; private set;} = 10.0f;

    [field: SerializeField] public float deccelarition { get; private set; } = 1.0f;
    [field: SerializeField] public float jump_strength {get; private set;} = 20.0f;
    [field: SerializeField] public float jump_damping {get; private set;} = 0.1f;
    [field: SerializeField] public float jump_time_length {get; private set;} = 2.0f;

    [field: SerializeField] public int jump_buffer_nb_frames {get; private set;} = 4;
    
    [field: SerializeField] public float dash_velocity {get; private set;} = 1.0f;
    [field: SerializeField] public float dash_time {get; private set;} = 1.0f;
    [field: SerializeField] public float attack_time {get; private set;} = 1.0f;
    [field: SerializeField] public float bounce_velocity_conversion_coef {get; private set;} = 0.2f;


}
