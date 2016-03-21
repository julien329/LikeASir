using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class MapHandler : MonoBehaviour
{
    [SerializeField]
    private int respawnTime = 3;
    public GameObject playerPrefab;

    Queue<Spawnable> spawnPoints;
    List<Spawnable> spawnPointsMartini;
    List<Spawnable> spawnPointsHats;
    List<GameObject> martinisList;
    List<GameObject> hatsList;

    static List<IPlatform> platforms;
    //Espace de texte
    public static List<GameObject> players;
    Image[] playerPanels;
    GameObject[] playerMartini;
    Text[] playerDeaths;
    Color[] playerColors;

    void Start()
    {
        players = new List<GameObject>();
        playerPanels = new Image[4];
        playerDeaths = new Text[4];
        playerColors = new Color[4];
        playerMartini = new GameObject[4];

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

        //Get UI elements 
        //Stat panels
        playerPanels[0] = GameObject.Find("Player1UI").GetComponent<Image>();
        playerPanels[1] = GameObject.Find("Player2UI").GetComponent<Image>();
        playerPanels[2] = GameObject.Find("Player3UI").GetComponent<Image>();
        playerPanels[3] = GameObject.Find("Player3UI").GetComponent<Image>();

        //Item display
        playerMartini[0] = GameObject.Find("ItemsP1");
        playerMartini[1] = GameObject.Find("ItemsP2");
        playerMartini[2] = GameObject.Find("ItemsP3");
        playerMartini[3] = GameObject.Find("ItemsP4");

        //Get Text elements
        playerDeaths[0] = GameObject.Find("DeathCountP1").GetComponent<Text>();
        playerDeaths[1] = GameObject.Find("DeathCountP2").GetComponent<Text>();
        playerDeaths[2] = GameObject.Find("DeathCountP3").GetComponent<Text>();
        playerDeaths[3] = GameObject.Find("DeathCountP4").GetComponent<Text>();

        //Create Players
        playerColors[0] = Color.red;
        playerColors[1] = Color.green;
        playerColors[2] = Color.blue;
        playerColors[3] = Color.yellow;

        //We'll consider automatic playerCount of 4 for now, we can change that in previous scene probly!
        players.Add((GameObject)Instantiate(playerPrefab, GetNextSpawn(), playerPrefab.transform.rotation));
        players.Add((GameObject)Instantiate(playerPrefab, GetNextSpawn(), playerPrefab.transform.rotation));
        players.Add((GameObject)Instantiate(playerPrefab, GetNextSpawn(), playerPrefab.transform.rotation));
        players.Add((GameObject)Instantiate(playerPrefab, GetNextSpawn(), playerPrefab.transform.rotation));

        for(int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerController>().playerNumber = i + 1;
            players[i].transform.FindChild("Head").GetComponent<MeshRenderer>().material.color = playerColors[i];
        }

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
        player.deathCount++;
        playerDeaths[player.playerNumber - 1].text = "Died: " + player.deathCount;
        playerPanels[player.playerNumber - 1].color = new Color(255, 255, 255, 1f);

        CannonScript cannon = players[player.playerNumber - 1].GetComponentInChildren<CannonScript>();
        //Dead player loses all his items!
        for (int i = 0; i < 3; i++) {
            player.martiniList[i] = false;
            UpdateItemDisplay(player);
        }
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
        playerPanels[player.playerNumber - 1].color = new Color(255, 255, 255, 100f/255);
    }

    public void UpdateItemDisplay(PlayerController player)
    {
        //GLASS
        //OLIVE
        //URANIUM

        int victory = 0;
        for (int i = 0; i < 3; i++)
        {
            if(player.martiniList[i])
            {
                playerMartini[player.playerNumber-1].transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;
                victory++;
            }
            else
                playerMartini[player.playerNumber-1].transform.GetChild(i).gameObject.GetComponent<Image>().enabled = false;
        }

        if (victory >= 3)
            PlayerWins(player);
    }

    Vector3 GetNextSpawn() {

        Spawnable spawn;

        do {
            spawn = spawnPoints.Dequeue();
            spawnPoints.Enqueue(spawn);
        }
        while (!spawn.isSpawnable);

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
}

