using UnityEngine;
using System;
using System.Collections.Generic;


public class WeaponBase : IPickup {
  
    [SerializeField]
    protected int maxAmmo;
    public int MaxAmmo { get { return maxAmmo; } }
    protected int ammo;
    public int Ammo { get { return ammo; } set { ammo = value; } }
    public string wepName;

    public GameObject projectile;


    void Start() {
        base.init();
    }


    // Allows to set the stats on a weapon being equiped
    public void initWeapon(WeaponBase wep) {
        wepName = wep.wepName;
        maxAmmo = wep.maxAmmo;
        ammo = maxAmmo;
        projectile = wep.projectile;
        isPickedUp = true;
    }


    // When a player picks up the item
    protected override void PickedUp(PlayerController player) {
        // Set the player weapon this weapon
        player.cannon.SetWeapon(this);

        // Destroy the floating pickup in game
        Destroy(gameObject);

        player.playerDisplay.UpdateWeaponDisplay(player);
    }
}

