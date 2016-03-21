using UnityEngine;
using System.Collections;

public class DynamicCamera : MonoBehaviour {

    public float visibleOffset = 5;
    public float moveSmoothIntensity = 5;
    float minX, minY, maxX, maxY;
    float zoomLevel;
    Camera mainCamera;
    Vector3 center;

    // Use this for initialization
    void Start () {
        mainCamera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        FindBounds();
        center = FindCenter();
        zoomLevel = FindRequiredZoom();

        Vector3 targetPos = new Vector3(center.x, center.y, -zoomLevel);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothIntensity * Time.deltaTime);
    }


    Vector3 FindCenter() {
        float centerX, centerY;

        if (minX == Mathf.Infinity || maxX == -Mathf.Infinity)
            centerX = 0;
        else
            centerX = (minX + maxX) / 2;


        if (minY == Mathf.Infinity || maxY == -Mathf.Infinity)
            centerY = 10;
        else
            centerY = (minY + maxY) / 2;

        return new Vector3(centerX, centerY);
    }


    void FindBounds() {
        minX = minY = Mathf.Infinity;
        maxX = maxY = -Mathf.Infinity;

        if (MapHandler.players != null) {
            foreach (GameObject player in MapHandler.players) {
                if (player != null && player.activeSelf) {
                    minX = Mathf.Min(minX, player.transform.position.x);
                    minY = Mathf.Min(minY, player.transform.position.y);
                    maxX = Mathf.Max(maxX, player.transform.position.x);
                    maxY = Mathf.Max(maxY, player.transform.position.y);

                }
            }
        }
    }

    float FindRequiredZoom() {
        float height = maxY - minY + (2 * visibleOffset);
        float newZoom = height * 0.5f / Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        newZoom = Mathf.Clamp(newZoom, 10, 50);

        return newZoom;
    }
}
