using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class MapHandler : MonoBehaviour
{
    [SerializeField]
    private int respawnTime = 3;
    public GameObject playerPrefab;

    //Will hold UI elements of the players
    public PlayerStats playerStats;

    Queue<Spawnable> spawnPoints;
    List<Spawnable> spawnPointsMartini;
    List<Spawnable> spawnPointsHats;
    List<GameObject> martinisList;
    List<GameObject> hatsList;

    public Color[] colors;

    static List<IPlatform> platforms;
    //Espace de texte
    public static List<GameObject> players;

    void Start()
    {
        playerStats = GameObject.Find("GameFlow").GetComponent<PlayerStats>();
        spawnPoints = new Queue<Spawnable>();
        for (int i = 0; i < 4; i++) {
            spawnPoints.Enqueue(GameObject.Find("PlayerSpawns").transform.GetChild(i).GetComponent<Spawnable>());
        }

        spawnPointsMartini = new List<Spawnable>();
        for (int i = 0; i < 6; i++) {
            spawnPointsMartini.Add(GameObject.Find("MartiniSpawns").transform.GetChild(i).GetComponent<Spawnable>());
        }

        martinisList = new List<GameObject>();
        martinisList.Add((GameObject)Resources.Load("Martini/MartiniClean"));
        martinisList.Add((GameObject)Resources.Load("Martini/OliveClean"));
        martinisList.Add((GameObject)Resources.Load("Martini/UraniumClean"));


        spawnPointsHats = new List<Spawnable>();
        for (int i = 0; i < 6; i++) {
            spawnPointsHats.Add(GameObject.Find("HatSpawns").transform.GetChild(i).GetComponent<Spawnable>());
        }

        hatsList = new List<GameObject>();
        hatsList.Add((GameObject)Resources.Load("Hats/FedoraClean"));
        hatsList.Add((GameObject)Resources.Load("Hats/TopHatClean"));

        spawnPlayers(Gameflow.playersInGame);

        StartCoroutine(ActivatePlatforms());
        StartCoroutine(SpawnMartinis());
        StartCoroutine(SpawnHats());
    }

    public static void AddToList(IPlatform platform)
    {
        if (platforms == null)
            platforms = new List<IPlatform>();
        platforms.Add(platform);
    }

    public List<IPlatform> GetList()
    {
        return platforms;
    }

    public void PlayerWins(PlayerController player)
    {
        Gameflow.GameWon(player);
    }

    public void PlayerDied(PlayerController player)
    {
        playerStats.PlayerDied(player);

        //What is this line about?--------------------------------------------------------------------------
        CannonScript cannon = players[player.playerNumber - 1].GetComponentInChildren<CannonScript>();

        //Dead player loses all his items!
        for (int i = 0; i < 3; i++) {
            player.martiniList[i] = false;
            
        }
        playerStats.UpdateItemDisplay(player);

        //POSSIBLE BM TEXT

        player.gameObject.SetActive(false);
        StartCoroutine(RespawnTime(player));
    }

    IEnumerator RespawnTime(PlayerController player)
    {
        int timer = 0;
        while(timer < respawnTime)
        {
            yield return new WaitForSeconds(1f);
            timer++;
        }

        player.transform.position = GetNextSpawn();
        player.gameObject.SetActive(true);
        playerStats.UpdatePanelColor(player);
    }

    

    Vector3 GetNextSpawn() {

        Spawnable spawn;
        // temp fix for infinite loop
        int iterations = 0;
        //
        do {
            spawn = spawnPoints.Dequeue();
            spawnPoints.Enqueue(spawn);
            iterations++;
        }
        while (!spawn.isSpawnable && iterations < 4);

        return spawn.position;
    }

    IEnumerator ActivatePlatforms() {
        yield return new WaitForSeconds(15f);

        while (true) {
            platforms[Random.Range(0, platforms.Count)].ApplyEffect();
            yield return new WaitForSeconds(Random.Range(5f, 15f));
        }
    }

    IEnumerator SpawnMartinis() {
        yield return new WaitForSeconds(15f);

        while (true) {
            Instantiate(martinisList[Random.Range(0,3)], spawnPointsMartini[Random.Range(0,6)].position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 15f));
        }
    }

    IEnumerator SpawnHats() {
        yield return new WaitForSeconds(15f);

        while (true) {
            Instantiate(hatsList[Random.Range(0, 2)], spawnPointsHats[Random.Range(0, 6)].position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 15f));
        }
    }

    public void spawnPlayers(List<int> playersInGame)
    {
        players = new List<GameObject>();

        //Inits the UI element tables
        playerStats.initTables(playersInGame.Count);

        //Get UI elements 
        //Stat panels
        //Get Text elements
        //Set colors (These are set in the editor)
        //Spawn the players
        for (int i = 0; i < playersInGame.Count; i++)
        {
             
            players.Add((GameObject)Instantiate(playerPrefab, GetNextSpawn(), playerPrefab.transform.rotation));
            players[i].GetComponent<PlayerController>().playerNumber = i + 1;
            playerStats.initPlayerUI(i);
            players[i].transform.FindChild("Head").GetComponent<MeshRenderer>().material.color = playerStats.playerColors[i];
        }
        Debug.Log(playersInGame.Count);
        //Erase the displays of the absent players
        for(int i = playersInGame.Count; i < 4; i++)
        {
            GameObject.Find("Player" + (i + 1) + "UI").SetActive(false);
        }

    }
}

