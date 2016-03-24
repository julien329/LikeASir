using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public abstract class IPickup : MonoBehaviour {

    protected int lifeTime = 10;
    protected bool isPickedUp = false;
    public bool IsPickedUp { get { return isPickedUp; } set { isPickedUp = value; } }

    // Object picked up by player
    protected abstract void PickedUp(PlayerController player);


    //Base init
    protected virtual void init(){
        if(!isPickedUp)
            StartCoroutine(DestroyTimer());
    }


    // Used to destroy the item if not picked during lifetime
    protected IEnumerator DestroyTimer() {
        int timer = 0;
        while (timer < lifeTime) {
            timer++;
            yield return new WaitForSeconds(1f);
        }
        Destroy(this.gameObject);
    }


    // On entering collision with other collider
    protected void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player" && !isPickedUp) 
            PickedUp(col.gameObject.GetComponent<PlayerController>());
    }
}

