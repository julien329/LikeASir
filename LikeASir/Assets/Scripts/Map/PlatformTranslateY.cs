using UnityEngine;
using System.Collections;

public class PlatformTranslateY : IPlatform {

    public float translateSpeed = 15f;
    public float returnTimer = 3f;
    public float targetDistance = 15f;


    void Start() {
        init();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P) && !inUse)
            ApplyEffect();
    }


    public override void ApplyEffect() {
        StartCoroutine(TranslatePlatform());
        inUse = true;
    }

    IEnumerator TranslatePlatform() {

        Vector3 origin = transform.position;
        Vector3 target = origin + new Vector3(0f, targetDistance, 0f);

        if (origin.y < target.y) {
            while (transform.position.y < target.y) {
                transform.Translate(Vector3.up * Time.deltaTime * translateSpeed);
                yield return null;
            }
        }
        if(origin.y > target.y) {
            while (transform.position.y > target.y) {
                transform.Translate(-Vector3.up * Time.deltaTime * translateSpeed);
                yield return null;
            }
        }

        transform.position = target;
        yield return new WaitForSeconds(returnTimer);

        if (transform.position.y < origin.y) {
            while (transform.position.y < origin.y) {
                transform.Translate(Vector3.up * Time.deltaTime * translateSpeed);
                yield return null;
            }
        }
        if (transform.position.y > origin.y) {
            while (transform.position.y > origin.y) {
                transform.Translate(-Vector3.up * Time.deltaTime * translateSpeed);
                yield return null;
            }
        }

        transform.position = origin;
        inUse = false;
    }
}
