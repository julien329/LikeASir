using UnityEngine;
using System.Collections;

public abstract class IPlatform : MonoBehaviour {

    protected bool inUse = false;

    public virtual void init()
    {
        MapHandler.AddToList(this);
    }

    public abstract void ApplyEffect();
}
