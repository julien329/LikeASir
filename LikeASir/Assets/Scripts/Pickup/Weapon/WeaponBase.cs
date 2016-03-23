using UnityEngine;
using System;
using System.Collections.Generic;


public class WeaponBase : IPickup
{
    public string wepName;

    [SerializeField]
    protected int maxAmmo;
    public int MaxAmmo { get { return maxAmmo; } }
    protected int ammo;
    public int Ammo { get { return ammo; } set { ammo = value; } }

    public GameObject projectile;


    void Start()
    {
        base.init();
        ammo = maxAmmo;
    }

    //Allows to set the stats on a weapon being equiped
    public void initWeapon(WeaponBase wep)
    {
        wepName = wep.wepName;
        maxAmmo = wep.maxAmmo;
        ammo = wep.ammo;
        projectile = wep.projectile;
        isPickedUp = true;
        StopCoroutine(LifeTime());
    }

    //Shoots the weapon from the cannon that it is equiped to
    public void Shoot(CannonScript cannon)
    {
        //Create the bullet
        GameObject bullet = Instantiate(projectile) as GameObject;
        bullet.transform.position = cannon.transform.position + cannon.transform.right;

        //Launch the bullet from the cannon
        bullet.GetComponent<IProjectile>().Launch(cannon.transform.right, cannon.player.gameObject.GetInstanceID());

        //Play the sound
        cannon.playSound();

        //Decrement the bullet count
        ammo--;
        if (ammo < 1)
        {
            cannon.setWeapon(null);
            Destroy(this);
        }
        
    }

    //What happens when a player picks up the item
    protected override void playerPickUp(PlayerController player)
    {
        //Set weapon to hat
        player.cannon.setWeapon(this);

        //Destroy the floating pickup on the play area
        Destroy(this.gameObject);

        player.playerDisplay.UpdateWeaponDisplay(player);
    }

    //What can trigger the pickup
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Entered");
        if(col.gameObject.tag == "Player")
        {
            playerPickUp(col.gameObject.GetComponent<PlayerController>());
        }
    }

}

