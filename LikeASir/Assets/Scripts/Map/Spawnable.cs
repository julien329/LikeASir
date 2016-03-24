using UnityEngine;
using System.Collections;

public enum SpawnType { PLAYER, MARTINI, HAT }

public class Spawnable : MonoBehaviour {

    public SpawnType spawnType;
    public bool isSpawnable = true;
    public Vector3 spawnPosition {
        get { return transform.position; }
    }


    void Start() {
        switch(spawnType) {
            case SpawnType.PLAYER :
                MapHandler.spawnPointsPlayers.Enqueue(this);
                break;
            case SpawnType.MARTINI :
                MapHandler.spawnPointsMartinis.Add(this);
                break;
            case SpawnType.HAT :
                MapHandler.spawnPointsHats.Add(this);
                break;
        }  
    }


    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
            isSpawnable = false;
    }


    void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
            isSpawnable = true;
    }
	
}
