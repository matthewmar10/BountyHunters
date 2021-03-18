using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    private Animator anim;

    // Human(0), Alien(1), Robot(2)
    private int raceSelection = 0;

    //Animator controllers for each race
    public RuntimeAnimatorController humanController;
    public RuntimeAnimatorController alienController;
    public RuntimeAnimatorController robotController;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Allows you to change race of player
    public void SwitchRace(int raceSelected)
    {
        switch (raceSelected)
        {
            //Switches player to human
            case 0:
                raceSelection = 0;
                anim.runtimeAnimatorController = humanController as RuntimeAnimatorController;
                break;

            //Switches player to alien
            case 1:
                raceSelection = 1;
                anim.runtimeAnimatorController = alienController as RuntimeAnimatorController;
                break;

            //Switches player to robot
            case 2:
                raceSelection = 2;
                anim.runtimeAnimatorController = robotController as RuntimeAnimatorController;
                break;
        }
    }
}
