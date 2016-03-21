using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroMenuWindow : MonoBehaviour {

    public static bool[] playersPlaying;
    public int playerNumber;
    static public bool beginLevel;

    Text playerReadyTxt;
    Text pressToPlayTxt;
    GameObject check;

	// Use this for initialization
	void Start () {
        beginLevel = false;
        if(playersPlaying == null)
            playersPlaying = new bool[4];
        playerReadyTxt = transform.GetChild(1).gameObject.GetComponent<Text>();
        check = transform.GetChild(3).gameObject;
        pressToPlayTxt = transform.GetChild(2).gameObject.GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetButtonDown("Fire" + playerNumber))
            playerReady();    
	}

    static public int nPlayersReady()
    {
        int ready = 0;
        foreach (bool x in playersPlaying)
            if(x)ready++;
        return ready;
    }

    void playerReady()
    {
        playersPlaying[playerNumber] = true;
        playerReadyTxt.text = "Ready!";
        pressToPlayTxt.text = "Press Start to play!";
        check.SetActive(true);
    }

    void StartGame()
    {
        beginLevel = true; 
       
    }

}
