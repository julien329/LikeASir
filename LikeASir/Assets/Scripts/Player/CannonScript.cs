using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour
{
    AudioSource audioSource;
    public PlayerController player;
    Transform cannonAim;
    public WeaponBase currentWeapon;
    bool aimUp;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = transform.root.GetComponent<PlayerController>();
        aimUp = false;
        cannonAim = transform;
        currentWeapon = null;
    }

    void Update()
    {
        //Calculate the triggerAxis
        float h = Input.GetAxisRaw("LeftVertical" + player.playerNumber);

        //InputAxis y going up = aim up if not already up
        if(h < 0 && !aimUp)
        {
            aimUp = true;
            adjustAim();
        }
        else if(h >= 0 && aimUp)
        {
            aimUp = false;
            adjustAim();
        }

        //On fire button: it shoots. :  negative y = right if it has a weapon
        if (transform.root.forward == new Vector3(1, 0, 0) || transform.root.forward == new Vector3(-1, 0, 0))
        {
            //If we have a weapon, we shoot it.
            if (currentWeapon != null && Input.GetButtonDown("Fire" + player.playerNumber))
                currentWeapon.Shoot(this);

        }
    }

    //Adjust Cannon aim if holding up
    void adjustAim()
    {
        if (aimUp)
            transform.Rotate(0f, 0f, -45f);
        else
            transform.Rotate(0f, 0f, 45f);
    }

    //Equips the weapon to the cannon.
    public void setWeapon(WeaponBase wep)
    {
        //If the weapon passed is null
        if (wep == null)
        {
            currentWeapon = null;
            Destroy(GetComponent<WeaponBase>());
            return;
        }

        //Destroy previous weapon if any
        if (GetComponent<WeaponBase>() != null)
            Destroy(GetComponent<WeaponBase>());

        //Create a new weapon script, attach it, and initiate it (Copy construction basically)
        currentWeapon = gameObject.AddComponent<WeaponBase>();
        currentWeapon.initWeapon(wep);
        
    }

    public void playSound()
    {
        audioSource.clip = currentWeapon.projectile.GetComponent<IProjectile>().shootBulletSound;
        audioSource.Play();
    }
}
