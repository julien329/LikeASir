using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public abstract class IPickup : MonoBehaviour
{
    protected int lifeTime = 10;
    protected bool isPickedUp;
    public bool IsPickedUp { get { return isPickedUp; } set { isPickedUp = value; } }
    protected GameObject player;

    public abstract void playerPickUp(PlayerController player);

    //
    public virtual void startTimer()
    {
        StartCoroutine(LifeTime());
    }

    public virtual void onTimerEnd()
    {
        if (!isPickedUp)
            Destroy(this.gameObject);
    }

    IEnumerator LifeTime()
    {
        int timer = 0;
        while (timer < lifeTime)
        {
            timer++;
            yield return new WaitForSeconds(1f);
        }
        onTimerEnd();
    }
}

