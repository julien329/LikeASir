using UnityEngine;
using System.Collections;

public class PlatformResizeX : IPlatform {

    public float resizeSpeed = 15f;
    public float returnTimer = 3f;
    public float targetSizeX = 4f;


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

        Vector3 original = transform.localScale;
        Vector3 target = new Vector3(targetSizeX, original.y, original.z);

        if (original.x < target.x) {
            while (transform.localScale.x < target.x) {
                transform.localScale = transform.localScale + Vector3.right * Time.deltaTime * resizeSpeed;
                yield return null;
            }
        }

        if (original.x > target.x) {
            while (transform.localScale.x > target.x) {
                transform.localScale = transform.localScale - Vector3.right * Time.deltaTime * resizeSpeed;
                yield return null;
            }
        }

        transform.localScale = target;
        yield return new WaitForSeconds(returnTimer);

        if (transform.localScale.x < original.x) {
            while (transform.localScale.x < original.x) {
                transform.localScale += Vector3.right * Time.deltaTime * resizeSpeed;
                yield return null;
            }
        }
        if (transform.localScale.x > original.x) {
            while (transform.localScale.x > original.x) {
                transform.localScale += -Vector3.right * Time.deltaTime * resizeSpeed;
                yield return null;
            }
        }

        transform.localScale = original;
        inUse = false;
    }
}
