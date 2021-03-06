﻿using UnityEngine;
using System.Collections;

public abstract class IPlatform : MonoBehaviour {

    protected bool inUse = false;

    public virtual void init() {
        MapHandler.platforms.Add(this);
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            ApplyEffect();
    }


    public abstract void ApplyEffect();
}
