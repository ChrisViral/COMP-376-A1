using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace A1
{
    public enum GameScenes
    {
        GAME = 0
    }

    [DisallowMultipleComponent]
    public class GameLogic : MonoBehaviour
    {
        public static GameLogic Instance { get; private set; }
        public static GameScenes CurrentScene { get; private set; }

        [SerializeField]
        private Vector3 spawn;
        [SerializeField]
        private GameObject[] asteroids;
        [SerializeField]
        private Vector2 spawnTimeRange;
        [SerializeField]
        private int maxSpawn, startWait;

        private int score;
        private bool gameEnded;
        private Text scoreLabel, restartLabel, gameoverLabel;
        private Coroutine spawner;

        public void UpdateScore(int added)
        {
            this.score += added;
            this.scoreLabel.text = $"Score: {this.score}";
        }

        public void OnStartGame(Scene scene, LoadSceneMode mode)
        {
            Debug.Log(scene.buildIndex);
            if (scene.buildIndex == (int)GameScenes.GAME)
            {
                //Get labels
                Canvas canvas = FindObjectOfType<Canvas>();
                List<Text> labels = new List<Text>();
                canvas.GetComponentsInChildren(true, labels);
                this.scoreLabel = labels.Find(l => l.name == "Score");
                this.restartLabel = labels.Find(l => l.name == "Restart");
                this.gameoverLabel = labels.Find(l => l.name == "Gameover");

                this.score = 0;
                this.gameEnded = false;
                this.spawner = StartCoroutine(SpawnAsteroids());
            }
        }

        public void EndGame()
        {
            this.gameEnded = true;
            StopCoroutine(this.spawner);
            this.gameoverLabel.gameObject.SetActive(true);
            this.restartLabel.gameObject.SetActive(true);
        }

        internal void LoadScene(GameScenes scene)
        {
            Debug.Log($"Loading scene {scene}");
            CurrentScene = scene;
            SceneManager.LoadScene((int)scene);
        }

        public void Quit()
        {
            Debug.Log("Exiting game...");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit()
#endif
        }

        private IEnumerator<YieldInstruction> SpawnAsteroids()
        {
            yield return new WaitForSeconds(this.startWait);

            while (true)
            {
                for (int i = 0, count = Random.Range(1, this.maxSpawn + 1); i < count; i++)
                {
                    Instantiate(this.asteroids[Random.Range(0, this.asteroids.Length)], new Vector3(Random.Range(this.spawn.x, this.spawn.y), 0f, this.spawn.z), Quaternion.identity);
                }
                yield return new WaitForSeconds(Random.Range(this.spawnTimeRange.x, this.spawnTimeRange.y));
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnStartGame;
        }

        private void Update()
        {
            if (this.gameEnded)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    LoadScene(GameScenes.GAME);
                }
            }
        }
    }
}
