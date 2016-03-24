using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IntroMenuWindow : MonoBehaviour {

    public static bool[] playersPlaying;
    public int playerNumber;
    static public bool beginLevel;
    Queue<Color> availableColors;

    Text playerNameTxt;
    Text playerReadyTxt;
    Text pressToPlayTxt;
    GameObject check;

    //public GameObject playerStatsObject;
    PlayerStats playerStats;
    Color currentColor = Color.gray;

    // Use this for initialization
    void Start () {
        playersPlaying = new bool[4];
        
        SetAvailableColors();

        playerStats = GameObject.Find("GameFlow").GetComponent<PlayerStats>();
        playerNameTxt = transform.GetChild(0).gameObject.GetComponent<Text>();
        playerReadyTxt = transform.GetChild(1).gameObject.GetComponent<Text>();
        pressToPlayTxt = transform.GetChild(2).gameObject.GetComponent<Text>();
        check = transform.GetChild(3).gameObject;
        
        UpdateTextColor();
    }


    // Add all available colors to the queue
    void SetAvailableColors() {
        availableColors = new Queue<Color>();

        availableColors.Enqueue(Color.red);
        availableColors.Enqueue(Color.blue);
        availableColors.Enqueue(Color.green);
        availableColors.Enqueue(Color.yellow);
        availableColors.Enqueue(Color.cyan);
        availableColors.Enqueue(Color.magenta);
        availableColors.Enqueue(Color.white);
    }


    // Update is called once per frame
    void Update () {  
        if (Input.GetButtonDown("Fire" + playerNumber)) {
            PlayerReady();
            ChangePlayerColor();
        }       
	}


    // Check how many players are ready to play
    static public int NbPlayersReady(){
        int nbReady = 0;
        foreach (bool player in playersPlaying) {
            if (player)
                nbReady++;
        }
        return nbReady;
    }


    // Put player in ready state
    void PlayerReady() {
        playersPlaying[playerNumber-1] = true;
        // Update player hud text and activate checkmark
        playerReadyTxt.text = "Ready!";
        pressToPlayTxt.text = "Press Start to play!";
        check.SetActive(true);
    }


    // Start the game
    void StartGame() {
        beginLevel = true; 
       
    }


    // Get next available color and update HUD color
    void ChangePlayerColor() {
        currentColor = NextColor();
        playerStats.playerColors[playerNumber - 1] = currentColor;
        UpdateTextColor();
    }


    // Change player hud color
    void UpdateTextColor() {
        playerNameTxt.color = currentColor;
        playerReadyTxt.color = currentColor;
        pressToPlayTxt.color = currentColor;
    }


    // Get next available color
    public Color NextColor() {
        Color newColor;
        do {
            newColor = availableColors.Dequeue();
            availableColors.Enqueue(newColor);
        } while (ColorIsInUse(newColor));

        return newColor;
    }


    //Checks if the color is currently used by the player.
    bool ColorIsInUse(Color color) {
        foreach (Color possibleColor in playerStats.playerColors)
            if (possibleColor.Equals(color))
                return true;
        return false;
    }
}
