using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.GetComponent<PlayerController>().playerDied();
        }
    }
}
