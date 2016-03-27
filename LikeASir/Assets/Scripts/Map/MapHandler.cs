using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class MapHandler : MonoBehaviour {
    public GameObject playerPrefab;
    public PlayerStats playerStats;
    public float initialSpawnDelay = 10f;
    public float minSpawnRate = 5f;
    public float maxSpawnRate = 15f;

    public static Queue<Spawnable> spawnPointsPlayers;
    public static List<Spawnable> spawnPointsMartinis;
    public static List<Spawnable> spawnPointsHats;
    public static List<GameObject> players;
    public static List<IPlatform> platforms;

    List<GameObject> martinisList;
    List<GameObject> hatsList;

    public Color[] colors;

    // Executed before Start()
    void Awake() {
        platforms = new List<IPlatform>();
        spawnPointsPlayers = new Queue<Spawnable>();
        spawnPointsMartinis = new List<Spawnable>();
        spawnPointsHats = new List<Spawnable>();

        martinisList = new List<GameObject>();
        hatsList = new List<GameObject>();
        players = new List<GameObject>();
        playerStats = GameObject.Find("GameFlow").GetComponent<PlayerStats>();
    }


    // Initiation
    void Start() {
        LoadRessources();

        StartCoroutine(ActivatePlatforms());
        StartCoroutine(SpawnMartinis());
        StartCoroutine(SpawnHats());
    }


    // Load gameObject in Ressources folder
    void LoadRessources() {
        martinisList.Add((GameObject)Resources.Load("Martini/MartiniClean"));
        martinisList.Add((GameObject)Resources.Load("Martini/OliveClean"));
        martinisList.Add((GameObject)Resources.Load("Martini/UraniumClean"));

        hatsList.Add((GameObject)Resources.Load("Hats/FedoraClean"));
        //hatsList.Add((GameObject)Resources.Load("Hats/TopHatClean"));
    }

    
    // Manage player deaths
    public void PlayerDied(PlayerController player) {
        playerStats.PlayerDied(player.playerNumber);

        // Dead player loses all his martinis and weapons
        player.martiniList = new bool[3];
        player.cannon.RemoveWeapon();
        playerStats.UpdateItemDisplay(player);
        playerStats.AddPoints(player.playerNumber, -10);

        // Desactive player and set respawn timer
        player.gameObject.SetActive(false);
        StartCoroutine(RespawnTime(player));
    }


    // Used to manage player respawn
    IEnumerator RespawnTime(PlayerController player) {
        int timer = 0;
        // Wait for the respawn time
        while (timer < player.respawnTime) {
            yield return new WaitForSeconds(1f);
            timer++;
        }

        // Spawn to random spawn point and reactivate player
        player.transform.position = GetNextSpawn();
        player.gameObject.SetActive(true);
        playerStats.UpdatePanelColor(player.playerNumber);
    }

    
    // Get the next free spawn point
    Vector3 GetNextSpawn() {
        Spawnable spawn;
        int iterations = 0;
        // Look for an free spawnPoint
        do {
            spawn = spawnPointsPlayers.Dequeue();
            spawnPointsPlayers.Enqueue(spawn);
            iterations++;
        }
        while (!spawn.isSpawnable && iterations < 4);

        return spawn.spawnPosition;
    }


    // Activate plaform effets randomly after initial delay
    IEnumerator ActivatePlatforms() {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true) {
            platforms[Random.Range(0, platforms.Count)].ApplyEffect();
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        }
    }


    // Spawn martinis pieces randomly after initial delay
    IEnumerator SpawnMartinis() {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true) {
            Instantiate(martinisList[Random.Range(0, martinisList.Count)], spawnPointsMartinis[Random.Range(0, spawnPointsMartinis.Count)].spawnPosition, Quaternion.identity);
            Martini.isSpawned = true;
            // Wait for the martini to be picked, before spawning an other one
            yield return new WaitWhile(() => Martini.isSpawned);
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        }
    }


    // Spawn hats randomly after initial delay
    IEnumerator SpawnHats() {
        yield return new WaitForSeconds(initialSpawnDelay);
;
        while (true) {
            Instantiate(hatsList[Random.Range(0, hatsList.Count)], spawnPointsHats[Random.Range(0, spawnPointsHats.Count)].spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        }
    }


    public void PrepareGame(List<IntroPlayerState> playersInfo) {
        // Initialize the UI element tables
        playerStats.InitTables(playersInfo.Count);

        // For every present players...
        for (int i = 0; i < playersInfo.Count; i++)
            playerStats.InitPlayerUI(i + 1);

        // Erase the displays of the absent players
        for (int i = playersInfo.Count; i < 4; i++)
            GameObject.Find("Player" + (i + 1) + "UI").SetActive(false);
    }

    // Initially spawn all present players
    public void SpawnPlayers(List<IntroPlayerState> playersInfo) {
        // For every present players..
        for (int i = 0; i < playersInfo.Count; i++) {
            // Instanciate prefab and add it to the list, then set correct player number and add the chosen color
            players.Add((GameObject)Instantiate(playerPrefab, GetNextSpawn(), playerPrefab.transform.rotation));
            players[i].transform.FindChild("Head").GetComponent<MeshRenderer>().material.color = playerStats.playerColors[i];
            players[i].GetComponent<PlayerController>().playerNumber = i + 1;
            players[i].GetComponent<PlayerController>().inputNumber = playersInfo[i].inputNumber;
        }
    }
}

