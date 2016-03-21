using UnityEngine;
using System.Collections;

public class gun_PowerUp : IWeaponBase {

    public AudioClip shootBulletSound;

    void Start()
    {
        wepName = "FedoraClean";
        startTimer();
    }

    public override void Shoot(CannonScript cannon)
    {
        cannon.player.PlaySound(shootBulletSound);

        GameObject bullet = (GameObject)Instantiate(Resources.Load("bullet")) as GameObject;
        bullet.transform.position = cannon.transform.position + cannon.transform.right * 2;
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        
        bulletScript.Launch(cannon.transform.right);
        
        ammo--;
        if (ammo == 0)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            cannon.currentWeapon = null;
        }
            
            
    }

    public override void playerPickUp(PlayerController player)
    {
        isPickedUp = true;
        ammo = 5;
        player.cannon.setWeapon(this);
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !isPickedUp)
        {
            player = col.gameObject;
            playerPickUp(col.gameObject.GetComponent<PlayerController>());
        }
    }

}
