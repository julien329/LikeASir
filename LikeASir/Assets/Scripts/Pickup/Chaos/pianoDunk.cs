using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class pianoDunk : ChaoticItem
{
    PlayerController player;

    void Start()
    {
        Init();
    }
    public override void bringMayhemToTheWorld()
    {
        int targetsSize = MapHandler.players.Count;
        GameObject target;
        do
        {
          int chosenOne = (int)UnityEngine.Random.Range(0f, (float)(targetsSize - 1));
          target = MapHandler.players[chosenOne];
        } while (target == player && target.activeSelf);
        GameObject piano = (GameObject)Instantiate(Resources.Load("piano")) as GameObject;
        piano.transform.position = target.transform.position + Vector3.up * 15;
        PianoScript pianoScript = piano.GetComponent<PianoScript>();
        pianoScript.piano = this;
    }

    protected override void PickedUp(PlayerController player)
    {
        this.player = player;
        bringMayhemToTheWorld();
        Destroy(gameObject);
    }
}
