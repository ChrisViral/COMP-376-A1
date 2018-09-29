using System.Collections.Generic;
using UnityEngine;

namespace A1
{
    public class GameLogic : MonoBehaviour
    {
        public static GameLogic Instance { get; private set; }

        [SerializeField]
        private Vector3 spawn;
        [SerializeField]
        private GameObject[] asteroids;
        [SerializeField]
        private Vector2 spawnTimeRange;
        [SerializeField]
        private int maxSpawn, startWait;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            StartCoroutine(SpawnAsteroids());
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
    }
}
