using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public abstract class ChaoticItem : IPickup
    {
        public MapHandler mapHandler;
        public abstract void bringMayhemToTheWorld();

        public virtual void Init()
        {
            mapHandler = GameObject.Find("MapHandler").GetComponent<MapHandler>();
        }
    }

