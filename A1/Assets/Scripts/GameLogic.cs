using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceShooter
{
    /// <summary>
    /// Game Scenes
    /// </summary>
    public enum GameScenes
    {
        GAME = 0
    }

    /// <summary>
    /// Game logic controller
    /// </summary>
    [DisallowMultipleComponent]
    public class GameLogic : MonoBehaviour
    {
        #region Instance
        /// <summary>
        /// GameLogic instance
        /// </summary>
        public static GameLogic Instance { get; private set; }
        #endregion

        #region Static Properties
        /// <summary>
        /// Currently loaded game scene
        /// </summary>
        public static GameScenes CurrentScene { get; private set; }
        #endregion

        #region Fields
        //Inspector fields
        [SerializeField]
        private Vector3 spawn;
        [SerializeField]
        private GameObject[] asteroids;
        [SerializeField]
        private Vector2 spawnTimeRange;
        [SerializeField]
        private int maxSpawn, startWait;

        //Private fields
        private int score;
        private bool gameEnded;
        private Text scoreLabel, restartLabel, gameoverLabel;
        private Coroutine spawner;  //Asteroid spawner coroutine
        #endregion

        #region Methods
        /// <summary>
        /// Updates the current player score by adding the specified amount of points
        /// </summary>
        /// <param name="added">Points to add</param>
        public void UpdateScore(int added)
        {
            this.score += added;
            this.scoreLabel.text = $"Score: {this.score}";
        }

        /// <summary>
        /// Game start event, loads the required elements and starts the spawn routines
        /// </summary>
        /// <param name="scene">Loaded scene</param>
        /// <param name="mode">Scene load mode</param>
        private void OnStartGame(Scene scene, LoadSceneMode mode)
        {
            //Only run if game scene is loaded
            if (scene.buildIndex == (int)GameScenes.GAME)
            {
                //Get labels
                Canvas canvas = FindObjectOfType<Canvas>();
                List<Text> labels = new List<Text>();
                canvas.GetComponentsInChildren(true, labels);
                this.scoreLabel = labels.Find(l => l.name == "Score");
                this.restartLabel = labels.Find(l => l.name == "Restart");
                this.gameoverLabel = labels.Find(l => l.name == "Gameover");

                //Setup game
                this.score = 0;
                this.gameEnded = false;

                //Start spawn coroutines
                this.spawner = StartCoroutine(SpawnAsteroids());
            }
        }

        /// <summary>
        /// Ends the game cycle
        /// </summary>
        public void EndGame()
        {
            //End game
            this.gameEnded = true;
            StopCoroutine(this.spawner);

            //Activate endgame text labels
            this.gameoverLabel.gameObject.SetActive(true);
            this.restartLabel.gameObject.SetActive(true);
        }

        /// <summary>
        /// Loads a given scene
        /// </summary>
        /// <param name="scene">Scene to load</param>
        internal void LoadScene(GameScenes scene)
        {
            Debug.Log($"Loading scene {scene}");
            CurrentScene = scene;
            SceneManager.LoadScene((int)scene);
        }

        /// <summary>
        /// Quits the game
        /// </summary>
        public void Quit()
        {
            Debug.Log("Exiting game...");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Asteroid spawning coroutine
        /// </summary>
        private IEnumerator<YieldInstruction> SpawnAsteroids()
        {
            //First wave delay
            yield return new WaitForSeconds(this.startWait);

            //Spawn loop
            while (true)
            {
                //Spawn wave
                for (int i = 0, count = Random.Range(1, this.maxSpawn + 1); i < count; i++)
                {
                    Instantiate(this.asteroids[Random.Range(0, this.asteroids.Length)], new Vector3(Random.Range(this.spawn.x, this.spawn.y), 0f, this.spawn.z), Quaternion.identity);
                }

                //Wait a random amount of time before spawning next wave
                yield return new WaitForSeconds(Random.Range(this.spawnTimeRange.x, this.spawnTimeRange.y));
            }
        }
        #endregion

        #region Functions
        private void Awake()
        {
            //Only allow one instance to exist
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            //Setup GameLogic instance
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnStartGame;
        }

        private void Update()
        {
            //Check for quit keypress
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Quit();
                return;
            }

            //If game ended, check for restart keypress
            if (this.gameEnded)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    LoadScene(GameScenes.GAME);
                }
            }
        }
        #endregion
    }
}
