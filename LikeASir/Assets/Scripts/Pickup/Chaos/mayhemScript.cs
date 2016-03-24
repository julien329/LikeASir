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
        foreach(IPlatform p in MapHandler.platforms)
        {
            p.ApplyEffect();
        }
    }

    protected override void PickedUp(PlayerController player)
    {
        bringMayhemToTheWorld();
        Destroy(gameObject);
    }
}
