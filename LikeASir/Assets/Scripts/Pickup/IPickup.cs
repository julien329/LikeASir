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

    protected abstract void playerPickUp(PlayerController player);

    //Base init
    protected virtual void init()
    {
        isPickedUp = false;
        startTimer();
    }

    protected virtual bool canBePicked(String tag)
    {
        if (Equals(tag, "Player") && !isPickedUp)
            return true;
        else
            return false;
    }

    public virtual void startTimer()
    {
        StartCoroutine(LifeTime());
    }

    public virtual void onTimerEnd()
    {
        if (this.gameObject != null)
            Destroy(this.gameObject);
    }

    protected IEnumerator LifeTime()
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

