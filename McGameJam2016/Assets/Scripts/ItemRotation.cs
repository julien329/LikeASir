using UnityEngine;
using System.Collections;

public class ItemRotation : MonoBehaviour {

    public float rotationSpeed;
    public float translationSpeed;
    public float translationDistance;

    Vector3 origin;

    void Start() {
        origin = transform.position;
        StartCoroutine(TranslateUp());
    }

    void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
	}

    IEnumerator TranslateUp() {
        Vector3 target = transform.position + new Vector3(0, translationDistance, 0);
        while (transform.position.y < target.y) {
            transform.Translate(Vector3.up * translationSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(TranslateDown());
    }

    IEnumerator TranslateDown() {
        Vector3 target = transform.position - new Vector3(0, translationDistance, 0);
        while (transform.position.y > target.y) {
            transform.Translate(Vector3.down * translationSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(TranslateUp());
    }
}
