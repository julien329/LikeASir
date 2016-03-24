using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchPosPowerUp : ChaoticItem
{
    public AudioClip teleportSound;


    public override void bringMayhemToTheWorld()
    {
        int targetsSize = MapHandler.players.Count;
        int bond = (int)Random.Range(1f, (float)(targetsSize - 1));

        Vector3[] positions = new Vector3[targetsSize];

        for (int i = 0; i < targetsSize; i++)
        {
            positions[i] = MapHandler.players[i].transform.position;
        }
        
        for (int i = 0; i < targetsSize; i++)
        {
            MapHandler.players[i].transform.position = positions[(i + bond) % 4];
        }

        MapHandler.players[0].GetComponent<PlayerController>().PlaySound(teleportSound);
    }

    protected override void PickedUp(PlayerController player)
    {
        bringMayhemToTheWorld();
        Destroy(gameObject);
    }
}