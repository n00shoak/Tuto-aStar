using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CL_Tile : CL_Manager
{
    // == General Data ==
    private GameObject model;
    private TileType tileType;
    private float maxHEalth, CurrentHealth;
    private bool broken;
    // *TD* task

    // == Wall Data == 
    private bool isGlass, isDoor;

    // == Object Data == 
    private bool isHermetic;

    // == Ground DATA  == 
    private float speedMultiplier;
    private float cleaningMultiplier, grime;


    // == General Method ==
    public void takeDamage(float Damage)
    {
        if(Damage-CurrentHealth <= 0) { broken = true;  }
        else { CurrentHealth -= Damage; }

        //create repair task  (this>tilemanager>managerManager>taskManager)
    }

    public void reapair(float healing)
    {
        CurrentHealth += healing;
        if (CurrentHealth > maxHEalth)
        {
            CurrentHealth = maxHEalth;
        }
    }

    /*wall method
      - open/close door => play door anim
    */


    public enum TileType 
    {
        Wall,
        Object,
        Ground,
    }
}

