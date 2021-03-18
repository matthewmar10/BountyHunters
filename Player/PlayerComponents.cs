using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public Rigidbody2D RigidBody
    {
        get
        {
            return rb;
        }
    }
}
