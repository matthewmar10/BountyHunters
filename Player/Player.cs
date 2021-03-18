using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStats stats;

    [SerializeField]
    private PlayerComponents components;

    private PlayerReferences references;
    private PlayerController controller;
    private PlayerUtilities utilities;

    public PlayerComponents Components
    {
        get
        {
            return components;
        }
    }

    public PlayerStats Stats 
    {
        get
        {
            return stats;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        utilities.HandleInput();
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        //controller = new PlayerController(this);
        utilities = new PlayerUtilities(this);
    }
}
