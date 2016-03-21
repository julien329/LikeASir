using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float power = 15f;
    public float lifeTime = 2f;
    public float gravity = -15f;

    void Start()
    {
        
    }

    void Update()
    {
        GetComponent<Rigidbody>().AddForce(0, gravity, 0, ForceMode.Acceleration);
    }

    public void Launch(Vector3 angle)
    {
        GetComponent<Rigidbody>().AddForce(angle * power, ForceMode.Impulse);
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
            Destroy(gameObject, 0.1f);
    }
}

