using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Gameflow : MonoBehaviour {

    enum GameState
    {
        INTRO,
        PREFIGHT,
        FIGHT,
        POSTFIGHT,
        ENDING

    }

    static MapHandler mapHandler;
    static DynamicCamera camScript;
    static GameState gameState;
    static Text textMod;
    static string textPure;
    static float countDown;
    static float gameTimer;
    static bool isLoaded;

    AudioSource audioSource;
    public AudioClip introMenuClip, introGameClip, loopClip;

    static public List<int> playersInGame;
    static Pair<GameObject, int>[] leaderBoard; //Player heads and their score. 100 to begin, -1 per death, + 100 for Martinist
    static public PlayerStats playerStats;

    /****************/
    //VictoryScript
    /****************/
    VictoryScript victoryScript;


    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playersInGame = new List<int>();
        StopAllCoroutines();
        DontDestroyOnLoad(this.gameObject);
        leaderBoard = new Pair<GameObject, int>[4];
        for (int i = 0; i < 4; i++)
            leaderBoard[i] = new Pair<GameObject, int>();

        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayMusicMenu());
    }

    public static void nextScene()
    {
        if (SceneManager.GetActiveScene().name == "IntroScene")
        {         
            SceneManager.LoadScene(1);
            textMod = null;
            countDown = 2f;
            gameTimer = 60f;

        }
    }

    void Update()
    {
        if (gameState == GameState.INTRO) // Intro Screen
        {
            if (Input.GetButtonDown("Start") && IntroMenuWindow.nPlayersReady() > 0)
            {
                //Check which players are playing
                for (int i = 0; i < 4; i++)
                    if (IntroMenuWindow.playersPlaying[i])
                        playersInGame.Add(i);
                nextScene();
            }
                
        }
        if (gameState == GameState.PREFIGHT) //Game on countdown
        {
            CountDown();
        }
        if (gameState == GameState.FIGHT) // Game running, timer on top.
        {
            //Display the player UI when the game starts
            GameObject.Find("UICamera").GetComponent<Camera>().cullingMask |= (1 << 10);
            GameTimer();
        }
        if (gameState == GameState.POSTFIGHT)
            CountDown2();
        if (gameState == GameState.ENDING)
        {
            //Loads the beginning of the game
            if (Input.GetButtonDown("Start"))
            {
                gameState = GameState.INTRO;
                SceneManager.LoadScene(0);
                Destroy(this.gameObject);
            }
                
        }
    }
    void OnLevelWasLoaded(int i)
    {
        if (i == 1)
        {
            StopCoroutine(PlayMusicMenu());
            StartCoroutine(PlayMusicGame());
            gameState = GameState.PREFIGHT; //Preparing Game
            mapHandler = GameObject.Find("MapHandler").GetComponent<MapHandler>();
            textMod = GameObject.Find("ReadyMessage").GetComponent<Text>();
            camScript = GameObject.Find("Main Camera").GetComponent<DynamicCamera>();

            textPure = textMod.text;
        }
        if (i == 2)
        {
            victoryScript = GameObject.Find("VictoryStuff").GetComponent<VictoryScript>();
            setPodium();
            gameState = GameState.ENDING;

        }
    }

    void CountDown()
    {
        countDown -= Time.deltaTime;
        int num = (int)countDown;
        textMod.text = textPure + "\n" + num.ToString();
        if (countDown < 0)
        {
            isLoaded = false;
            mapHandler.enabled = true;
            textMod.transform.parent.gameObject.SetActive(false);
            camScript.enabled = true;
            textMod = GameObject.Find("Timer").GetComponent<Text>();
            //textPure = textMod.text;
            gameState = GameState.FIGHT;

        }
    }

    void CountDown2()
    {
        countDown -= Time.deltaTime;
        int num = (int)countDown;
        textMod.text = textPure + "\n" + num.ToString();
        if (countDown < 0)
        {

            SceneManager.LoadScene(2);
        }
    }

    void GameTimer()
    {
        gameTimer -= Time.deltaTime;
        int num = (int)gameTimer;
        textMod.text = num.ToString();

        if (gameTimer < 0)
        {
            gameState = GameState.POSTFIGHT;
            GameFinished();

        }
    }

    public static void GameWon(PlayerController player)
    {
        textMod.text = "Player " + player.playerNumber + " is the Martinist!";
        countDown = 3f;
        fillLeaderBoard(player.GetComponent<PlayerController>().playerNumber);

    }

    void GameFinished()
    {
        textMod.text = "Time is up!";
        fillLeaderBoard(5);
        countDown = 3f;
    }

    //Fills an ordered leaderboard
    static void fillLeaderBoard(int winner)
    {

        camScript.enabled = false;
        for (int i = 0; i < 4; i++)
        {
            DontDestroyOnLoad(MapHandler.players[i]);
            leaderBoard[i].First = new GameObject();
            Debug.Log(leaderBoard.Length);
            Debug.Log(MapHandler.players[i].name);
            leaderBoard[i].First = MapHandler.players[i];
            leaderBoard[i].Second = 100 - MapHandler.players[i].GetComponent<PlayerController>().deathCount;
            if (winner < 4)
                leaderBoard[winner].Second += 100;
            MapHandler.players[i].SetActive(false); //Not sure about that one
        }

        //Order LeaderBoard;
        Pair<GameObject, int>[] leaderBoardOrdered = new Pair<GameObject, int>[4];
        Pair<GameObject, int> bestScore;

        for (int j = 0; j < 4; j++)
        {
            bestScore = leaderBoard[0];
            for (int i = 1; i < 4; i++)
            {
                if (bestScore.Second < leaderBoard[i].Second)
                    bestScore = leaderBoard[i];
            }
            leaderBoardOrdered[j] = bestScore;
            bestScore.Second = -10001;
        }

        leaderBoard = leaderBoardOrdered;

    }

    public void setPodium()
    {
        victoryScript.playerColors = new Color[4];
        victoryScript.playerHeads = new GameObject[4];
        victoryScript.playerPlaces = new int[4];
        victoryScript.playerNumber = 4;

        Text[] victoryTexts = GameObject.Find("ScoreBoard").GetComponentsInChildren<Text>();
        //Create Players
        victoryScript.playerColors[0] = Color.red;
        victoryScript.playerColors[1] = Color.green;
        victoryScript.playerColors[2] = Color.blue;
        victoryScript.playerColors[3] = Color.yellow;
        for (int i = 0; i < 4; i++)
        {
            victoryScript.playerPlaces[i] = leaderBoard[i].First.GetComponent<PlayerController>().playerNumber;
            victoryScript.playerHeads[i] = leaderBoard[i].First.transform.GetChild(0).gameObject;


            if (leaderBoard[i].Second != -10000)
                victoryTexts[i].text = victoryTexts[i].text + " " + victoryScript.playerPlaces[i];
            else
                victoryTexts[i].text = "Player " + victoryScript.playerPlaces[i] + " as Sir-Not-Appearing-In-This-Game";
            Destroy(leaderBoard[i].First);
        }

        GameObject.Find("robot1").transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = victoryScript.playerColors[victoryScript.playerPlaces[0] - 1];
        GameObject.Find("robot2").transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = victoryScript.playerColors[victoryScript.playerPlaces[1] - 1];
        GameObject.Find("robot3").transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = victoryScript.playerColors[victoryScript.playerPlaces[2] - 1];
        GameObject.Find("robot4").transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = victoryScript.playerColors[victoryScript.playerPlaces[3] - 1];
    }


    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };

    IEnumerator PlayMusicMenu() {
        audioSource.clip = introMenuClip;
        audioSource.Play();
        audioSource.loop = false;

        yield return new WaitForSeconds(introMenuClip.length);

        audioSource.clip = loopClip;
        audioSource.Play();
        audioSource.loop = true;
    }

    IEnumerator PlayMusicGame() {

        audioSource.clip = introGameClip;
        audioSource.Play();
        audioSource.loop = false;

        yield return new WaitForSeconds(introGameClip.length);

        audioSource.clip = loopClip;
        audioSource.Play();
        audioSource.loop = true;
    }

}
