using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroMenuWindow : MonoBehaviour {

    public static bool[] playersPlaying;
    public int playerNumber;
    static public bool beginLevel;

    Text playerNameTxt;
    Text playerReadyTxt;
    Text pressToPlayTxt;
    GameObject check;

    //Color Select
    public GameObject playerStatsObject;
    PlayerStats playerStats;
    Color currentColor;

    // Use this for initialization
    void Start () {
        if(playersPlaying == null)
            playersPlaying = new bool[4];

        playerStats = playerStatsObject.GetComponent<PlayerStats>();
        playerNameTxt = transform.GetChild(0).gameObject.GetComponent<Text>();

        currentColor = Color.gray;

        beginLevel = false;

        playerReadyTxt = transform.GetChild(1).gameObject.GetComponent<Text>();
        check = transform.GetChild(3).gameObject;
        pressToPlayTxt = transform.GetChild(2).gameObject.GetComponent<Text>();
        updateTextColor();

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetButtonDown("Fire" + playerNumber))
        {
            playerReady();
            changePlayerColor();
        }
                
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
        playersPlaying[playerNumber-1] = true;
        playerReadyTxt.text = "Ready!";
        pressToPlayTxt.text = "Press Start to play!";
        check.SetActive(true);
    }

    void StartGame()
    {
        beginLevel = true; 
       
    }

    void changePlayerColor()
    {
        currentColor = playerStats.nextColor(currentColor, playerNumber);
        updateTextColor();
    }

    void updateTextColor()
    {
        playerNameTxt.color = currentColor;
        playerReadyTxt.color = currentColor;
        pressToPlayTxt.color = currentColor;
    }

}
