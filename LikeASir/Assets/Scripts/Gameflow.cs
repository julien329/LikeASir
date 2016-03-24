using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

enum GameState { INTRO, PREFIGHT, FIGHT, POSTFIGHT, ENDING
}

public class Gameflow : MonoBehaviour {

    static MapHandler mapHandler;
    static DynamicCamera camScript;
    static GameState gameState;
    static Text textMod;
    static string textPure;
    static float countDown;
    static float gameTimer;

    AudioSource audioSource;
    public AudioClip introMenuClip, introGameClip, loopClip;

    static public List<int> playersInGame;
    static Pair<GameObject, int>[] leaderBoard; //Player heads and their score. 100 to begin, -1 per death, + 100 for Martinist
    static public PlayerStats playerStats;


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
            if (Input.GetButtonDown("Start") && IntroPlayerState.NbPlayersReady() > 0)
            {
                //Check which players are playing
                for (int i = 0; i < 4; i++)
                    if (IntroPlayerState.playersPlaying[i])
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
        //Add 50 points to the winner
        playerStats.playerScores[player.playerNumber - 1] += 50;

        countDown = 3f;
        playerStats.fillLeaderBoard();

    }

    void GameFinished()
    {
        textMod.text = "Time is up!";
        playerStats.fillLeaderBoard();
        countDown = 3f;
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


    // Play the menu music
    IEnumerator PlayMusicMenu() {
        audioSource.clip = introMenuClip;
        audioSource.Play();
        audioSource.loop = false;

        yield return new WaitForSeconds(introMenuClip.length);

        audioSource.clip = loopClip;
        audioSource.Play();
        audioSource.loop = true;
    }


    // Play the in game music
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
