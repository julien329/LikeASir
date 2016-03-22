using UnityEngine;
using System.Collections;

public class DrownScript : ChaoticItem
{

    public override void bringMayhemToTheWorld()
    {
        GameObject drowner = (GameObject)Instantiate(Resources.Load("drowner")) as GameObject;
    }

    protected override void playerPickUp(PlayerController player)
    {
        isPickedUp = true;
        bringMayhemToTheWorld();
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !isPickedUp)
        {
            player = col.gameObject;
            playerPickUp(col.gameObject.GetComponentInChildren<PlayerController>());
        }
    }

    
}
