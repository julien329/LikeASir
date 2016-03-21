using UnityEngine;
using System.Collections;

public class Drowner : MonoBehaviour {
    GameObject acidLake;
    Vector3 initPos;
    float direction = 1, height, riseDistance = 8f, time = 4f;

    void Start () {
        acidLake = GameObject.Find("Water");
        initPos = acidLake.transform.position;

        StartCoroutine(WaterUp());
    }

    IEnumerator WaterUp() {
        print(initPos);
        Vector3 targetPos = initPos + Vector3.up * riseDistance;
        while ( acidLake.transform.position.y < targetPos.y) {
            acidLake.transform.Translate(Vector3.up * (riseDistance / time));
            yield return null;
        }
        StartCoroutine(WaterDown());
    }

    IEnumerator WaterDown()
    {
        while (acidLake.transform.position.y > initPos.y)
        {
            acidLake.transform.Translate(-Vector3.up * Time.deltaTime * (riseDistance / time));
            yield return null;
        }
        acidLake.transform.position = initPos;
    }
}
