using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour
{
    public PlayerController player;
    Transform cannonAim;
    public IWeaponBase currentWeapon;
    bool aimUp;

    // Use this for initialization
    void Start()
    {
        player = transform.root.GetComponent<PlayerController>();
        aimUp = false;
        cannonAim = transform;
        currentWeapon = null;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("RightVertical" + player.playerNumber);
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
        // 
        //On fire button: it shoots. :  negative y = right if it has a weapon
        if (transform.root.forward == new Vector3(1, 0, 0) || transform.root.forward == new Vector3(-1, 0, 0))
        {
        if (currentWeapon != null && Input.GetButtonDown("Fire" + player.playerNumber))
            {

                currentWeapon.Shoot(this);

            }
        }
    }

    //Adjust Cannon aim
    void adjustAim()
    {
        if (aimUp)
            transform.Rotate(0f, 0f, -45f);
        else
            transform.Rotate(0f, 0f, 45f);
    }

    public void setWeapon(IWeaponBase wep)
    {
        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        currentWeapon = transform.FindChild(wep.wepName).GetComponentInChildren<IWeaponBase>();
        currentWeapon.transform.parent.gameObject.SetActive(true);
        currentWeapon.Ammo = wep.Ammo;
        Destroy(wep.transform.parent.gameObject);

    }
}
