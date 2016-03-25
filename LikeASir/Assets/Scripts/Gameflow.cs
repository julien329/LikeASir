using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

enum GameState { INTRO, PREFIGHT, FIGHT, POSTFIGHT, ENDING, LOADING }

public class Gameflow : MonoBehaviour {

    static MapHandler mapHandler;
    static DynamicCamera mainCamera;
    static GameState gameState;
    static Text textMod;
    static string textPure;
    static int countDown;
    static int gameTimer;

    AudioSource audioSource;
    public AudioClip introMenuClip, introGameClip, loopClip;

    static public List<int> playersInGame;
    static Pair<GameObject, int>[] leaderBoard; //Player heads and their score. 100 to begin, -1 per death, + 100 for Martinist
    static public PlayerStats playerStats;


    void Start() {
        DontDestroyOnLoad(this.gameObject);
        playerStats = GetComponent<PlayerStats>();
        playersInGame = new List<int>();
        leaderBoard = new Pair<GameObject, int>[4];
        for (int i = 0; i < 4; i++)
            leaderBoard[i] = new Pair<GameObject, int>();

        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Intro());
        StartCoroutine(PlayMusicMenu());
    }


    IEnumerator Intro() {
        while(gameState == GameState.INTRO) {
            if (Input.GetButtonDown("Start") && IntroPlayerState.NbPlayersReady() > 0) {
                //Check which players are playing
                for (int i = 0; i < 4; i++) {
                    if (IntroPlayerState.playersPlaying[i])
                        playersInGame.Add(i);
                }
                gameState = GameState.LOADING;
            }
            yield return null;
        }
        StartCoroutine(Prefight());
    }


    IEnumerator Prefight() {
        SceneManager.LoadScene(1);

        yield return new WaitUntil(() => gameState == GameState.PREFIGHT);

        countDown = 2;
        textMod.text = textPure + "\n" + countDown.ToString();

        while (gameState == GameState.PREFIGHT) {
            yield return new WaitForSeconds(1f);

            countDown--;
            textMod.text = textPure + "\n" + countDown.ToString();

            if (countDown == 0) {
                yield return new WaitForSeconds(1f);
                textMod.text = textPure + "\n" + "Fight!";
                yield return new WaitForSeconds(1f);

                mapHandler.enabled = true;
                textMod.transform.parent.gameObject.SetActive(false);
                mainCamera.enabled = true;
                textMod = GameObject.Find("Timer").GetComponent<Text>();
                gameState = GameState.FIGHT;   
            }
            
        }
        StartCoroutine(Fight());
    }


    IEnumerator Fight() {
        GameObject.Find("UICamera").GetComponent<Camera>().cullingMask |= (1 << 10);
        gameTimer = 10;
        textMod.text = gameTimer.ToString();  

        while (gameState == GameState.FIGHT) {
            yield return new WaitForSeconds(1f);

            gameTimer--;
            textMod.text = gameTimer.ToString();

            if (gameTimer == 0) {
                textMod.text = "Time is up!";
                //playerStats.fillLeaderBoard();  // NOT WORKING
                gameState = GameState.POSTFIGHT;
            }
        }
        StartCoroutine(PostFight());
    }


    IEnumerator PostFight() {
        countDown = 3;
        textMod.text = textPure + "\n" + countDown.ToString();
        
        while (gameState == GameState.POSTFIGHT) {
            yield return new WaitForSeconds(1f);

            countDown--;
            textMod.text = textPure + "\n" + countDown.ToString();

            if (countDown == 0)
                gameState = GameState.LOADING;
        }
        StartCoroutine(Ending());
    }
          
       
    IEnumerator Ending() {
        SceneManager.LoadScene(2);

        yield return new WaitUntil(() => gameState == GameState.ENDING);

        while (gameState == GameState.ENDING) {
            yield return null;

            if (Input.GetButtonDown("Start")) {
                gameState = GameState.INTRO;
                SceneManager.LoadScene(0);
                Destroy(this.gameObject);
            }
        }
    }


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


    void OnLevelWasLoaded(int i) {
        if (i == 1) {
            StopCoroutine(PlayMusicMenu());
            StartCoroutine(PlayMusicGame());
            gameState = GameState.PREFIGHT;
            mapHandler = GameObject.Find("MapHandler").GetComponent<MapHandler>();
            textMod = GameObject.Find("ReadyMessage").GetComponent<Text>();
            mainCamera = GameObject.Find("Main Camera").GetComponent<DynamicCamera>();

            textPure = textMod.text;
        }
        if (i == 2) {
            gameState = GameState.ENDING;
        }
    }


    ///////////////////NOT CHECKED//////////////////////


    public static void GameWon(PlayerController player)
    {
        textMod.text = "Player " + player.playerNumber + " is the Martinist!";
        //Add 50 points to the winner
        playerStats.playerScores[player.playerNumber - 1] += 50;

        countDown = 3;
        playerStats.fillLeaderBoard();

    }

    void GameFinished() {
        textMod.text = "Time is up!";
        playerStats.fillLeaderBoard();
        countDown = 3;
    }

    public class Pair<T, U> {
        public Pair() { }

        public Pair(T first, U second) {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };
}
