using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

enum ColorChoice { GREY, BLUE, CYAN, GREEN, MAGENTA, RED, WHITE, YELLOW }


public class PlayerStats : MonoBehaviour {

    public Color[] playerColors;
    public int[] playerScores;

    Image[] playerPanels;
    Text[] playerNames;
    GameObject[] playerMartini;
    Text[] playerDeaths;
    Text[] playerAmmo;
    Text[] playerScoreText;


    void Start() {    
        playerColors = new Color[4] { Color.grey, Color.grey, Color.grey, Color.grey };
    }

	
    // Sets the game scene table sizes for all the present players
    public void initTables(int nPlayers)
    {
        playerNames = new Text[nPlayers];
        playerPanels = new Image[nPlayers];
        playerDeaths = new Text[nPlayers];
        //playerColors = new Color[nPlayers];
        playerMartini = new GameObject[nPlayers];
        playerAmmo = new Text[nPlayers];
        playerScoreText = new Text[nPlayers];
    }

    //Initiates the display for a specific player
    public void initPlayerUI(int i)
    {
        //playerColors[i] = Gameflow.playerStats.playerColors[i];
        GameObject UI = GameObject.Find("Player" + (i + 1) + "UI");
        playerPanels[i] = UI.GetComponent<Image>();

        //Panel Text Colors and initiation:
        playerNames[i] = GameObject.Find("NameP" + (i + 1)).GetComponent<Text>();
        playerNames[i].color = playerColors[i];

        playerDeaths[i] = GameObject.Find("DeathCountP" + (i + 1)).GetComponent<Text>();
        playerDeaths[i].color = playerColors[i];

        playerScoreText[i] = GameObject.Find("ScoreP" + (i + 1)).GetComponent<Text>();
        playerScoreText[i].color = playerColors[i];
        playerScoreText[i].text = "Score: " + playerScores[i];

        playerAmmo[i] = GameObject.Find("WeaponP" + (i + 1)).GetComponent<Text>();
        playerAmmo[i].color = playerColors[i];

        //Martini Panels
        playerMartini[i] = GameObject.Find("ItemsP" + (i + 1));
    }

    

    void colorsSelected()
    {

    }

    public void addPoints(int playerNum, int points)
    {
        playerScores[playerNum - 1] += points;
        updateScoreDisplay();
    }

    public static void updateScoreDisplay()
    {

    }

    public void PlayerDied(PlayerController player)
    {
        player.deathCount++;
        playerDeaths[player.playerNumber - 1].text = "Died: " + player.deathCount;
        playerPanels[player.playerNumber - 1].color = new Color(255, 255, 255, 1f);
    }

    public void UpdateItemDisplay(PlayerController player)
    {
        //GLASS
        //OLIVE
        //URANIUM

        int victory = 0;
        for (int i = 0; i < 3; i++)
        {
            if (player.martiniList[i])
            {
                playerMartini[player.playerNumber - 1].transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;
                victory++;
            }
            else
                playerMartini[player.playerNumber - 1].transform.GetChild(i).gameObject.GetComponent<Image>().enabled = false;
        }

        if (victory >= 3)
            Gameflow.GameWon(player);
    }

    public void UpdatePanelColor(PlayerController player)
    {
        playerPanels[player.playerNumber - 1].color = new Color(255, 255, 255, 100f / 255);
    }

    public void UpdateWeaponDisplay(PlayerController player)
    {
        if (player.cannon.currentWeapon != null)
            playerAmmo[player.playerNumber - 1].text = "Ammo: " + player.cannon.currentWeapon.Ammo + "/" + player.cannon.currentWeapon.MaxAmmo;
        else
            playerAmmo[player.playerNumber - 1].text = "";//
    }

    //Fills an ordered leaderboard
    public List<int> fillLeaderBoard()
    {
        List<int> rawScores = new List<int>();
        List<int> playerNumbers = new List<int>();
        List<int> playerOrder = new List<int>();

        //Store the player numbers and respective scores
        for(int i = 0; i < playerScores.Length; i++)
        {
            rawScores.Add(playerScores[i]);
            playerNumbers.Add(playerNumbers[i]);
        }

        //Sorts the scores
        rawScores.Sort();

        //Finds the scores in descending order, removing the found player from the list after each iteration
        for(int i = rawScores.Count-1; i >=0; i--)
        {
            for(int j = playerNumbers.Count - 1; j >= 0; j--)
            if(playerScores[j] == rawScores[i])
            {
                playerOrder.Add(playerNumbers[j]);
                playerNumbers.RemoveAt(j);
            }
        }
        return playerOrder;
    }
}
