using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtilities
{
    private Player player;

    private List<Command> commands = new List<Command>();

    public PlayerUtilities(Player p)
    {
        player = p;
        /*
        commands.Add(new WeaponSwapCommand(player, WEAPON.DAGGER, KeyCode.Alpha1));
        commands.Add(new WeaponSwapCommand(player, WEAPON.SWORD, KeyCode.Alpha2));
        */
    }

    public void HandleInput()
    {
        //Need to add movement directions
        //player.Stats.moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        foreach (Command command in commands)
        {
           // if (Input.GetKeyDown(command.Key))
            
        }
    }
}
