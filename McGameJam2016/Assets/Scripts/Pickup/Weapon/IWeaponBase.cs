using UnityEngine;
using System;
using System.Collections.Generic;


public abstract class IWeaponBase : IPickup
{
    public string wepName;
    protected int ammo;
    public int Ammo { get { return ammo; } set { ammo = value; } }

    public abstract void Shoot(CannonScript cannon);

    public override void onTimerEnd()
    {
        if (!isPickedUp && transform.root.gameObject.tag != "Player")
            Destroy(this.transform.parent.gameObject);
    }


}

