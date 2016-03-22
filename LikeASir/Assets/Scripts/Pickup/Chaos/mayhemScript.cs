using UnityEngine;
using System.Collections;

public class mayhemScript : ChaoticItem
{
    void Start()
    {
        Init();
    }
    public override void bringMayhemToTheWorld()
    {
        foreach(IPlatform p in mapHandler.GetList())
        {
            p.ApplyEffect();
        }
    }

    protected override void playerPickUp(PlayerController player)
    {
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
