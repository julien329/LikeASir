using UnityEngine;
using System.Collections.Generic;

public enum MartiniPiece { GLASS, OLIVE, URANIUM, TOTAL }


public class Martini : IPickup
{
    public MartiniPiece martiniType;
    public AudioClip glassSound, oliveSound, uraniumSound;
    public static bool isSpawned = false;
    int pickUpPoints = 50;

    protected override void PickedUp(PlayerController player)
    {
        // Play sound according to the picked martini
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

        // If the player doesn't have the piece, add to his inventory and update UI
        if (!player.martiniList[(int)martiniType]) {
            player.martiniList[(int)martiniType] = true;
            player.mapHandler.playerStats.UpdateItemDisplay(player);
        }

        // Add points for picking up a martini
        player.playerDisplay.AddPoints(player.playerNumber, pickUpPoints);

        // Destroy object in game
        isSpawned = false;
        Destroy(this.gameObject);       
    }
}

