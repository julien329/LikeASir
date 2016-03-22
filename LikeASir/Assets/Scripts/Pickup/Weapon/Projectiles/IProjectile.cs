using UnityEngine;
using System.Collections;

public abstract class IProjectile : MonoBehaviour {

    protected Rigidbody myBody;
    public AudioClip shootBulletSound;
    public float power = 45;
    public float lifeTime = 2f;
    public float gravity = -9f;

    protected int shooterID;

    //Virtual init: Most projectiles will want to do this - Simply affect the correct component references
    protected virtual void init()
    {
        myBody = GetComponent<Rigidbody>();
    }

    //ApplyGravity to the projectile
    protected virtual void applyGravity()
    {
        myBody.AddForce(0, gravity, 0, ForceMode.Acceleration);
    }

    //Standard launch function - No friendly fire on it and shoots straight.
    //Derived may redefine this class to shoot backwards or include self shooting
    public virtual void Launch(Vector3 angle, int shooterID)
    {
        init();
        this.shooterID = shooterID;
        myBody.AddForce(angle * power, ForceMode.Impulse);
        Destroy(gameObject, lifeTime);
    }

    //Defines the effect of the bullet on hit. Additionnal effects could be described in derived projectiles
    protected abstract void applyEffect(GameObject target, ContactPoint[] colPoints);
    
}

