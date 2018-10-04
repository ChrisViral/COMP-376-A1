using SpaceShooter.Scenes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    /// <summary>
    /// Game Scenes
    /// </summary>
    public enum GameScenes
    {
        MENU = 0,
        GAME = 1
    }

    /// <summary>
    /// Game difficulties
    /// </summary>
    public enum GameMode
    {
        NORMAL,
        HARD
    }

    /// <summary>
    /// Game logic controller
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(AudioSource))]
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

        /// <summary>
        /// Curent GameMode
        /// </summary>
        public static GameMode GameMode { get; internal set; }

        private static bool isPaused;
        /// <summary>
        /// If the game is currently paused
        /// </summary>
        public static bool IsPaused
        {
            get { return isPaused; }
            private set
            {
                isPaused = value;
                Time.timeScale = isPaused ? 0f : 1f;
            }
        }

        /// <summary>
        /// Current ongoing Game
        /// </summary>
        public static Game CurrentGame { get; private set; }
        #endregion

        #region Fields
        //Inspector fields
        [SerializeField, Header("Music")]
        private AudioClip menuMusic;
        [SerializeField]
        private AudioClip gameMusic;

        //Private fields
        private AudioSource source;
        #endregion

        #region Static methods
        /// <summary>
        /// Loads a given scene
        /// </summary>
        /// <param name="scene">Scene to load</param>
        internal static void LoadScene(GameScenes scene) => SceneManager.LoadScene((int)scene);

        /// <summary>
        /// Quits the game
        /// </summary>
        internal static void Quit()
        {
            Debug.Log("Exiting game...");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion

        #region Methods
        /// <summary>
        /// Game scene loaded event
        /// </summary>
        /// <param name="scene">Loaded scene</param>
        /// <param name="mode">Load mode</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameScenes loadedScene = (GameScenes)scene.buildIndex;
            switch (loadedScene)
            {
                    case GameScenes.MENU:
                        CurrentGame = null;
                        this.source.clip = this.menuMusic;
                        this.source.Play();
                        break;

                    case GameScenes.GAME:
                        CurrentGame = FindObjectOfType<Game>();
                        if (CurrentScene != GameScenes.GAME)
                        {
                            this.source.clip = this.gameMusic;
                            this.source.Play();
                        }
                        break;
            }
            Debug.Log($"Scene loaded: {loadedScene}");
            CurrentScene = loadedScene;
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
            SceneManager.sceneLoaded += OnSceneLoaded;

            //Setup audio
            this.source = GetComponent<AudioSource>();
            this.source.loop = true;
        }

        private void Update()
        {
            //Pauses the game
            if (CurrentScene == GameScenes.GAME && Input.GetKeyDown(KeyCode.Escape))
            {
                IsPaused = !IsPaused;
            }
        }
        #endregion
    }
}
