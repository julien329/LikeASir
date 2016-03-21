using UnityEngine;
using System.Collections;

public class magnet_PowerUp : IWeaponBase {

    public AudioClip magnetSound;

    void Start()
    {
        wepName = "TopHatClean";
        startTimer();
    }

    public override void Shoot(CannonScript cannon)
    {
        cannon.player.PlaySound(magnetSound);

        GameObject laser = (GameObject)Instantiate(Resources.Load("magnet")) as GameObject;
        laser.transform.position = cannon.transform.position + cannon.transform.right * 2;
        MagnetScript magnetScript = laser.GetComponent<MagnetScript>();
        magnetScript.Launch(cannon.transform.right);
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
        ammo = 2;
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
