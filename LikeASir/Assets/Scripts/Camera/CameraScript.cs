using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
    float neededWidth, neededHeight;
    public float smoothFactor, distance;
    Camera mainCamera;
    Vector3 center, velocity;

	void Start () {
        mainCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.position = new Vector3(transform.position.x, transform.position.y, -distance);
        neededWidth = FindMostRight() - FindMostLeft();
        neededHeight = FindMostUp() - FindMostDown();
        if (neededHeight < -10000 || neededWidth < -10000)
            transform.position = new Vector3(0, 12, -distance);
        else {
            center = FindCenter();
            velocity = new Vector3((center.x - transform.position.x) / smoothFactor,
                               (center.y - transform.position.y) / smoothFactor,
                                0);
            if (neededHeight >= neededWidth / (float)(mainCamera.pixelWidth / mainCamera.pixelHeight))
                distance = Mathf.Lerp(distance, neededHeight / 2 * 1.5f / Mathf.Tan(mainCamera.fieldOfView * Mathf.Deg2Rad / 2.2f), Time.deltaTime * smoothFactor);
            else
                distance = Mathf.Lerp(distance, neededWidth / ((float)mainCamera.pixelWidth / mainCamera.pixelHeight) / 2 * 1.5f / Mathf.Tan(mainCamera.fieldOfView * Mathf.Deg2Rad / 2.2f), Time.deltaTime * smoothFactor);
            if (distance < 10f)
                distance = 10f;
            transform.position += velocity;
        }
        
    }

    float FindMostRight()
    {
        float output = -1000000;
        foreach(GameObject G in MapHandler.players)
        {
            if (G.transform.position.x > output && G.activeSelf)
                output = G.transform.position.x;
        }
        return output;
    }
    float FindMostLeft()
    {
        float output = 1000000;
        foreach (GameObject G in MapHandler.players)
        {
            if (G.transform.position.x < output && G.activeSelf)
                output = G.transform.position.x;
        }
        return output;
    }
    float FindMostUp()
    {
        float output = -1000000;
        foreach (GameObject G in MapHandler.players)
        {
            if (G.transform.position.y > output && G.activeSelf)
                output = G.transform.position.y;
        }
        return output;
    }
    float FindMostDown()
    {
        float output = 1000000;
        foreach (GameObject G in MapHandler.players)
        {
            if (G.transform.position.y < output && G.activeSelf)
                output = G.transform.position.y;
        }
        return output;
    }
    Vector3 FindCenter()
    {
        Vector3 c = Vector3.zero;
        int count = 0;
        foreach (GameObject G in MapHandler.players)
        {
            if (G.activeSelf)
            {
                c += G.transform.position;
                count++;
            }
        }
        c /= count;
        c += (Mathf.Tan(transform.rotation.eulerAngles.x) * distance * Vector3.up);
        return c;
    }
}
