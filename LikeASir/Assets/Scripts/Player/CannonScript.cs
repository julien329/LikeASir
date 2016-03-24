using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour
{
    AudioSource audioSource;
    public PlayerController player;
    public WeaponBase currentWeapon;
    bool aimUp = false;

    // Initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
        player = transform.root.GetComponent<PlayerController>();
    }


    void Update() {
        // Calculate the triggerAxis
        float vertical = Input.GetAxisRaw("LeftVertical" + player.playerNumber);

        // If joystick up and not already aimed up, aim cannon up
        if(vertical < 0 && !aimUp) {
            aimUp = true;
            transform.Rotate(0f, 0f, -45f);
        }
        // If joystick not up and aimed up, aim cannon back to normal position
        else if (vertical >= 0 && aimUp) {
            aimUp = false;
            transform.Rotate(0f, 0f, 45f);
        }

        // If player is facing left or right (so it doesn't shoot in the z axis)...
        if (transform.root.forward == new Vector3(1, 0, 0) || transform.root.forward == new Vector3(-1, 0, 0)) {
            // If cannon has a weapon and fire button is pressed...
            if (currentWeapon != null && Input.GetButtonDown("Fire" + player.playerNumber)) {
                // Shoot and update the ammo display
                //currentWeapon.Shoot(this);
                Shoot();
                player.playerDisplay.UpdateWeaponDisplay(player);
            }
        }
    }


    // Shoots the current weapon
    public void Shoot() {
        // Instanciate the current weapon bullet in front of the player
        GameObject bullet = Instantiate(currentWeapon.projectile) as GameObject;
        bullet.transform.position = transform.position + transform.right;

        // Launch the bullet from the cannon
        bullet.GetComponent<IProjectile>().Launch(transform.right, player.gameObject.GetInstanceID());

        // Play the sound for the current weapon
        audioSource.clip = currentWeapon.projectile.GetComponent<IProjectile>().shootBulletSound;
        audioSource.Play();

        // Decrement the bullet count
        currentWeapon.Ammo--;

        // If no more bullet, remove the weapon
        if (currentWeapon.Ammo == 0) {
            RemoveWeapon();
        }
    }


    // Equips the weapon to the cannon.
    public void SetWeapon(WeaponBase wep) {
        //Destroy previous weapon if any
        if (currentWeapon != null)
            RemoveWeapon();

        //Create a new weapon script, attach it, and initiate it (Copy construction basically)
        currentWeapon = gameObject.AddComponent<WeaponBase>();
        currentWeapon.initWeapon(wep);    
    }


    // Remove current weapon
    public void RemoveWeapon() {
        Destroy(currentWeapon);
        currentWeapon = null;
        player.playerDisplay.UpdateWeaponDisplay(player);
    }
}
