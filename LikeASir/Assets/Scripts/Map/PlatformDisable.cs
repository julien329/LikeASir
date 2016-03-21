using UnityEngine;
using System.Collections;

public class PlatformDisable : IPlatform {

    public float returnTimer = 3f;

    Renderer mesh;
    Collider col;


    void Start() {
        mesh = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        init();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P) && !inUse)
            ApplyEffect();
    }


    public override void ApplyEffect() {
        StartCoroutine(DisablePlatform());
        inUse = true;
    }

    IEnumerator DisablePlatform() {

        mesh.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(returnTimer);

        mesh.enabled = true;
        col.enabled = true;

        inUse = false;
    }
}
