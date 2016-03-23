using UnityEngine;
using System.Collections;

public class PlatformTranslateY : IPlatform {

    public float translateSpeed = 750f;
    public float returnTimer = 3f;
    public float targetDistance = 15f;
    Rigidbody rigidBody;


    void Start() {
        init();
        rigidBody = GetComponent<Rigidbody>();
    }


    public override void ApplyEffect() {
        if (!inUse) {
            StartCoroutine(TranslatePlatform());
            inUse = true;
        }
    }


    IEnumerator TranslatePlatform() {
        
        Vector3 origin = transform.position;
        Vector3 target = origin + new Vector3(0f, targetDistance, 0f);
        rigidBody.constraints &= ~RigidbodyConstraints.FreezePositionY;

        if (origin.y < target.y) {
            while (transform.position.y < target.y) {
                rigidBody.velocity = new Vector3(0, translateSpeed * Time.deltaTime, 0);
                yield return null;
            }
        }
        if(origin.y > target.y) {
            while (transform.position.y > target.y) {
                rigidBody.velocity = new Vector3(0, -translateSpeed * Time.deltaTime, 0);
                yield return null;
            }
        }

        rigidBody.velocity = Vector3.zero;
        transform.position = target;
        rigidBody.constraints |= RigidbodyConstraints.FreezePositionY;

        yield return new WaitForSeconds(returnTimer);

        rigidBody.constraints &= ~RigidbodyConstraints.FreezePositionY;

        if (transform.position.y < origin.y) {
            while (transform.position.y < origin.y) {
                rigidBody.velocity = new Vector3(0, translateSpeed * Time.deltaTime, 0);
                yield return null;
            }
        }
        if (transform.position.y > origin.y) {
            while (transform.position.y > origin.y) {
                rigidBody.velocity = new Vector3(0, -translateSpeed * Time.deltaTime, 0);
                yield return null;
            }
        }

        rigidBody.velocity = Vector3.zero;
        transform.position = origin;

        rigidBody.constraints |= RigidbodyConstraints.FreezePositionY;
        inUse = false;
    }
}
