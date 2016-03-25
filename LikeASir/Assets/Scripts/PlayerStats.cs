using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour {

    public Color[] playerColors;
    public int[] playerScores;
    public int[] playerDeathCounts;

    GameObject[] UIMartini;
    Image[] UIPanels;
    Text[] UINames;
    Text[] UIDeaths;
    Text[] UIAmmo;
    Text[] UIScoreText;

    Gameflow gameflow;

    // Initialisations
    void Awake() {
        playerScores = new int[4];
        playerDeathCounts = new int[4];
        playerColors = new Color[4] { Color.grey, Color.grey, Color.grey, Color.grey };

        gameflow = GetComponent<Gameflow>();
    }

	
    // Sets the game scene table sizes for all the present players
    public void InitTables(int nPlayers) {
        UINames = new Text[nPlayers];
        UIPanels = new Image[nPlayers];
        UIDeaths = new Text[nPlayers];
        UIMartini = new GameObject[nPlayers];
        UIAmmo = new Text[nPlayers];
        UIScoreText = new Text[nPlayers];
    }


    // Initiates the UI display for a specific player
    public void InitPlayerUI(int playerNumber) {
        UIPanels[playerNumber - 1] = GameObject.Find("Player" + playerNumber + "UI").GetComponent<Image>();

        UINames[playerNumber - 1] = GameObject.Find("NameP" + playerNumber).GetComponent<Text>();
        UINames[playerNumber - 1].color = playerColors[playerNumber - 1];

        UIDeaths[playerNumber - 1] = GameObject.Find("DeathCountP" + playerNumber).GetComponent<Text>();
        UIDeaths[playerNumber - 1].color = playerColors[playerNumber - 1];

        UIScoreText[playerNumber - 1] = GameObject.Find("ScoreP" + playerNumber).GetComponent<Text>();
        UpdateScoreDisplay(playerNumber);

        UIAmmo[playerNumber - 1] = GameObject.Find("WeaponP" + playerNumber).GetComponent<Text>();
        UIAmmo[playerNumber - 1].color = playerColors[playerNumber - 1];

        UIMartini[playerNumber - 1] = GameObject.Find("ItemsP" + playerNumber);
    }


    // Add score points to array
    public void AddPoints(int playerNumber, int points) {
        playerScores[playerNumber - 1] += points;
        UpdateScoreDisplay(playerNumber);
    }


    // Update score UI for given player
    public void UpdateScoreDisplay(int playerNumber) {
        UIScoreText[playerNumber - 1].color = playerColors[playerNumber - 1];
        UIScoreText[playerNumber - 1].text = "Score: " + playerScores[playerNumber - 1];
    }


    // Manage deathCount for given player and update UI
    public void PlayerDied(int playerNumber) {
        playerDeathCounts[playerNumber - 1]++;
        UIDeaths[playerNumber - 1].text = "Died: " + playerDeathCounts[playerNumber - 1];
        UIPanels[playerNumber - 1].color = new Color(255, 255, 255, 1f);
    }


    // Update the martini items IU for given player
    public void UpdateItemDisplay(PlayerController player) {
        int martiniItems = 0;
        // For all martinis items...
        for (int i = 0; i < player.martiniList.Length; i++) {
            // If picked by the player, enable image in UI
            if (player.martiniList[i]) {
                UIMartini[player.playerNumber - 1].transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;
                martiniItems++;
            }
            // Else disable the image in UI
            else
                UIMartini[player.playerNumber - 1].transform.GetChild(i).gameObject.GetComponent<Image>().enabled = false;
        }
        // If all three martinis are collected by the player, stop the game
        if (martiniItems == 3)
            gameflow.GameWon(player);
    }


    // Restore semi-transparent panel color (after repawn)
    public void UpdatePanelColor(int playerNumber) {
        UIPanels[playerNumber - 1].color = new Color(255, 255, 255, 100f / 255);
    }


    // Update the weapon display for a given player
    public void UpdateWeaponDisplay(PlayerController player) {
        if (player.cannon.currentWeapon != null)
            UIAmmo[player.playerNumber - 1].text = "Ammo: " + player.cannon.currentWeapon.Ammo + "/" + player.cannon.currentWeapon.MaxAmmo;
        else
            UIAmmo[player.playerNumber - 1].text = "";
    }



    ///////////////////NOT CHECKED//////////////////////



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
