using UnityEngine;
using System.Collections;


public class Cannonball : IProjectile
{
    //Get the references of the object
    void Start()
    {
        base.init();
    }

    //Apply gravity to the object
    void Update()
    {
        applyGravity();
    }

    //Manage what the bullet can hit
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player" && col.gameObject.GetInstanceID() != shooterID)
        {
            applyEffect(col.gameObject, col.contacts);
        }
    }

    //Effect is applied when the target is hit
    //Target is pushed back
    //Bullet is destroyed
    protected override void applyEffect(GameObject player, ContactPoint[] colPoints)
    {
        player.GetComponent<Rigidbody>().AddForceAtPosition(myBody.velocity, colPoints[0].point, ForceMode.Impulse);
        Destroy(this.gameObject);
    }

}

