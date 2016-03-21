using UnityEngine;
using System.Collections;

public class PlatformRotateZ : IPlatform {

    public float rotateSpeed = 400f; 
    public float returnTimer = 3f;
    public float targetRotation = 90f;


    void Start()
    {
        init();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P) && !inUse)
            ApplyEffect();
    }


    public override void ApplyEffect() {
        StartCoroutine(RotatePlatform());
        inUse = true;
    }

    IEnumerator RotatePlatform() {

        Quaternion target = Quaternion.Euler(0f, 0f, targetRotation);
        Quaternion origin = transform.rotation;
        
        while (transform.rotation.z < target.z) {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
            yield return null;
        }

        transform.rotation = target;
        yield return new WaitForSeconds(returnTimer);

        while (transform.rotation.z > origin.z) {
            transform.Rotate(-Vector3.forward * Time.deltaTime * rotateSpeed);
            yield return null;
        }

        transform.rotation = origin;
        inUse = false;
    }
}
