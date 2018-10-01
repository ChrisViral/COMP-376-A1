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

        /// <summary>
        /// Current ongoing Game
        /// </summary>
        public static Game CurrentGame { get; private set; }
        #endregion

        #region Static methods
        /// <summary>
        /// Loads a given scene
        /// </summary>
        /// <param name="scene">Scene to load</param>
        internal static void LoadScene(GameScenes scene)
        {
            CurrentScene = scene;
            SceneManager.LoadScene((int)scene);
        }

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

        /// <summary>
        /// Game scene loaded event
        /// </summary>
        /// <param name="scene">Loaded scene</param>
        /// <param name="mode">Load mode</param>
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CurrentScene = (GameScenes)scene.buildIndex;
            Debug.Log($"Scene loaded: {CurrentScene}");

            switch (CurrentScene)
            {
                    case GameScenes.MENU:
                        CurrentGame = null;
                        //LoadScene(GameScenes.GAME);
                        break;

                    case GameScenes.GAME:
                        CurrentGame = FindObjectOfType<Game>();
                        break;
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
            Debug.Log("GameLogic created");
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        #endregion
    }
}
