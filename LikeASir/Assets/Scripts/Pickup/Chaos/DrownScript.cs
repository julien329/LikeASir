using UnityEngine;
using System.Collections;

public class DrownScript : ChaoticItem
{
    public override void bringMayhemToTheWorld() {
        Instantiate(Resources.Load("drowner"));
    }

    protected override void PickedUp(PlayerController player) {
        isPickedUp = true;
        bringMayhemToTheWorld();
        Destroy(gameObject);
    }
}
