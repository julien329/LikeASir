using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryScript : MonoBehaviour
{

    public GameObject[] playerHeads;
    public int[] playerPlaces;
    public Color[] playerColors;
    public int playerNumber;


    // Use this for initialization
    void Start()
    {
        


    }
}
    /*
    Text[] victoryTexts = GameObject.Find("ScoreBoard").GetComponentsInChildren<Text>();

        for(int i = 0; i< 4; i++)
        {
            //Set head colors
            
                playerHeads[playerPlaces[i]].GetComponent<MeshRenderer>().material.color = playerColors[playerPlaces[i]];
                victoryTexts[i].text = victoryTexts[i].text + " " + playerPlaces[i];

	*/
