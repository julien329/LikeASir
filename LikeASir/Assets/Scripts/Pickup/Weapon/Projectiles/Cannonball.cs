using UnityEngine;
using System.Collections;


public class Cannonball : IProjectile
{
    void Update() {
        // Apply gravity to the object
        applyGravity();
    }


    // On entering collision with an other collider
    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Player" && col.gameObject.GetInstanceID() != shooterID)
            applyEffect(col.gameObject, col.contacts);
    }


    //Effect is applied when the target is hit
    //Target is pushed back
    //Bullet is destroyed
    protected override void applyEffect(GameObject player, ContactPoint[] colPoints) {
        player.GetComponent<Rigidbody>().AddForceAtPosition(projectile.velocity, colPoints[0].point, ForceMode.Impulse);
        Destroy(this.gameObject);
    }

}

