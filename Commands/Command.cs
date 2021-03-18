using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command 
{
    //public KeyCode key { get; private set; }
    /*
    public Command (KeyCode k)
    {
        key = k;
    }
    */
    public virtual void GetKeyDown()
    {

    }

    //Executed once the key is lifted up
    public virtual void GetKeyUp()
    {

    }

    //Executed every frame the key is pressed
    public virtual void GetKey()
    {

    }
}
