using UnityEngine;
using System.Collections;

public class PlatformStandart : IPlatform {

    private Renderer mesh;
    public float returnTimer = 3f;

    void Start() {
        mesh = GetComponent<Renderer>();
        init();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P) && !inUse)
            ApplyEffect();
    }

    public override void ApplyEffect() {
        StartCoroutine(ColorPlatform());
        inUse = true;
    }

    IEnumerator ColorPlatform() {
        Color color = mesh.material.color;
        mesh.material.color = Color.green;
        yield return new WaitForSeconds(returnTimer);
        mesh.material.color = color;
        inUse = false;
    }
}
