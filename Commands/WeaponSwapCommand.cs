using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WEAPON {DAGGER, SWORD}

public class WeaponSwapCommand : Command
{
    private Player player;
    private WEAPON weapon;
    /*
    public WeaponSwapCommand(Player p, WEAPON w, KeyCode key) : base(key)
    {
        player = p;
        weapon = w;
    }
    
    public override void GetKeyDown()
    {
        player.Actions.TrySwapWeapon(weapon);
    }
    */
}
