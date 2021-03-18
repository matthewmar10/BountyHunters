using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats 
{
    public Vector2 moveDirection { get; set; }

    //public float Speed { get; set; }

    [SerializeField]
    private float speed;

    public float Speed
    {
        get
        {
            return speed;
        }
    }
}
