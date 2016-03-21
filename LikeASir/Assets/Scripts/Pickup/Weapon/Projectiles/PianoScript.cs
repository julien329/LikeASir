using UnityEngine;
using System.Collections;

public class PianoScript : MonoBehaviour
{
    public pianoDunk piano;
    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            piano.mapHandler.PlayerDied(col.gameObject.GetComponent<PlayerController>());
        }
    }
}

