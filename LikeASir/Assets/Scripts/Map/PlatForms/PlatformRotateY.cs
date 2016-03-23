using UnityEngine;
using System.Collections;

public class PlatformRotateY : IPlatform {

    public float rotateSpeed = 400f;
    public float returnTimer = 3f;
    public float targetRotation = 90f;

    void Start() {
        init();
    }


    public override void ApplyEffect() {
        if (!inUse) {
            StartCoroutine(RotatePlatform());
            inUse = true;
        }
    }


    IEnumerator RotatePlatform() {

        Quaternion target = Quaternion.Euler(0f, targetRotation, 0f);
        Quaternion origin = transform.rotation;

        while (transform.rotation.y < target.y) {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
            yield return null;
        }

        transform.rotation = target;
        yield return new WaitForSeconds(returnTimer);

        while (transform.rotation.y > origin.y) {
            transform.Rotate(-Vector3.up * Time.deltaTime * rotateSpeed);
            yield return null;
        }

        transform.rotation = origin;
        inUse = false;
    }
}
