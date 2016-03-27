using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IntroPlayerState : MonoBehaviour {

    public int playerNumber = 0;
    public int inputNumber = 0;

    Queue<Color> availableColors;
    Text playerNameTxt;
    Text playerReadyTxt;
    Text pressToPlayTxt;
    //GameObject check;
    Image panel;
    PlayerStats playerStats;
    Color currentColor = Color.gray;


    void Awake() {
        playerStats = GameObject.Find("GameFlow").GetComponent<PlayerStats>();
        playerNameTxt = transform.GetChild(0).gameObject.GetComponent<Text>();
        playerReadyTxt = transform.GetChild(1).gameObject.GetComponent<Text>();
        pressToPlayTxt = transform.GetChild(2).gameObject.GetComponent<Text>();
        //check = transform.GetChild(3).gameObject;
        panel = GetComponent<Image>();
    }


    void Start () {
        SetAvailableColors();    
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
        if (inputNumber != 0 && Input.GetButtonDown("Fire" + inputNumber))
            ChangePlayerColor();    
	}


    // Put player in ready state
    public void PlayerReady(int playerNb, int inputNb) {
        // Set input and player number
        inputNumber = inputNb;
        playerNumber = playerNb;

        // Update player hud text and activate checkmark
        playerReadyTxt.text = "Ready!";
        pressToPlayTxt.text = "Press Start to Begin!";
        //check.SetActive(true);

        // Get first available color
        ChangePlayerColor();
        panel.color = Color.white;
    }


    // Get next available color and update HUD color
    public void ChangePlayerColor() {
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


    // Checks if the color is currently used by the player.
    bool ColorIsInUse(Color color) {
        foreach (Color possibleColor in playerStats.playerColors)
            if (possibleColor.Equals(color))
                return true;
        return false;
    }
}
