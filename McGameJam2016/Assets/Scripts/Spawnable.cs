using UnityEngine;
using System.Collections;

public class Spawnable : MonoBehaviour {

    public bool isSpawnable = true;
    public Vector3 position {
        get { return transform.position; }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
            isSpawnable = false;
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
            isSpawnable = true;
    }
	
}
