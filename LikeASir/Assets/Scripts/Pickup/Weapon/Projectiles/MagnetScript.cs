using UnityEngine;
using System.Collections;

public class MagnetScript : MonoBehaviour {

    public float speed;
    public float power;
    public float rotateSpeed;
    private Vector3 direction;
    public bool isLaunched;
    GameObject caster;

    void Start()
    {
        rotateSpeed = 1000f;
        speed = 4f;
        power = 600f;
    }

    public void Launch(Vector3 dir)
    {
        isLaunched = true;
        direction = dir;
        Destroy(gameObject, 4f);
    }

    void Update()
    {
        if (isLaunched)
        {
            Travel();
        }
    }
    
    private void Travel()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.Rotate(rotateSpeed * Time.deltaTime, 0f, 0f);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" && col.gameObject != caster)
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(-direction *power);
            Destroy(gameObject, 0.1f);
        }
    }
}
