using UnityEngine;
using System.Collections.Generic;

public class Martini : IPickup
{
    public MartiniPiece martiniType;
    public AudioClip glassSound, oliveSound, uraniumSound;


    void Start()
    {
        startTimer();
    }

    protected override void playerPickUp(PlayerController player)
    {
        switch (martiniType) {
            case MartiniPiece.GLASS :
                player.PlaySound(glassSound);
                break;
            case MartiniPiece.OLIVE :
                player.PlaySound(oliveSound);
                break;
            case MartiniPiece.URANIUM :
                player.PlaySound(uraniumSound);
                break;
        }


        if (!player.martiniList[(int)martiniType])
        {
            isPickedUp = true;
            player.martiniList[(int)martiniType] = true;
            Destroy(this.gameObject);
            player.mapHandler.playerStats.UpdateItemDisplay(player);
        }
        else
            Destroy(this.gameObject);
            
    }

    public enum MartiniPiece
    {
        GLASS,
        OLIVE,
        URANIUM,
        TOTAL        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerPickUp(col.gameObject.GetComponent<PlayerController>());
        }
    }

}

