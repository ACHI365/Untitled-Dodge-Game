using System.Collections;
using Enemy;
using Player;
using UnityEngine;

namespace Managers
{
    public class ManageGame : MonoBehaviour
    {
        [Header("references")] 
        [SerializeField] private Spawner spawner;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private PlayerMovement player;
        [SerializeField] private MenuManager menu;
        [SerializeField] private UltimateAbility ultimateAbility;
        public GameObject deathMenu;
        public GameObject swordUI;

        [Header("gameState")] public float slowness = 10f;
        public static bool Paused;
        private bool gameIsStarted;
        private Touch touch;

        void Start()
        {
            spawner.firstSpawnTime = 0.8f; // start slowly
            spawner.secondSpawnTime = 0.7f;
        }

        public void StartGame()
        {
            Time.timeScale = 1f; // set time scale to normal
            scoreManager.scoreText.gameObject.SetActive(true); // show score
            menu.pauseImage.SetActive(true); // show pause button
            deathMenu.SetActive(false); // hide death plane
            gameIsStarted = true;
            menu.start.SetActive(false); // hide menu
            menu.setting.SetActive(false); // hide menu
            menu.settingsOnStart.SetActive(false); // hide menu
            swordUI.SetActive(true); // show sword
            scoreManager.gameObject.SetActive(true);
            spawner.gameObject.SetActive(true); // activate spawner
            spawner.InvokeBoxGenerator(); // start spawner
            player.gameIsStarted = true;
        }

        void Lose()
        {
            DestroyAllBoxesOnScreen();
            swordUI.SetActive(false);  // when lose hide swordUI
            player.gameIsStarted = false; 
            player.gameObject.SetActive(false);
            scoreManager.scoreText.gameObject.SetActive(false); // hide score text
            menu.pauseImage.SetActive(false); // hide pause button
            scoreManager.End(); // end score counting
        }

        public void Restart()
        {
            ultimateAbility.threshold = 30;
            spawner.counter = 0; // don't count anymore
            ScoreManager.Score = 0; // ScoreManager score is 0 now
            player.gameObject.SetActive(true);
            StartGame(); // start game again
            player.Restart(); // restart player preferences
        }

        void Update()
        {
            
            if (gameIsStarted)  // if game is on
            {
                // check for additional invokes
                if (spawner.invokers > spawner.disInvokers + 1)
                {
                    for (int i = 0; i < spawner.invokers - spawner.disInvokers; i++)
                    {
                        spawner.CancelInvoke();
                    }
                    spawner.InvokeBoxGenerator();
                }
                
                int currScore = ScoreManager.Score; // count current score
                if (currScore is > 10 and < 70) // after some time start aggressively
                {
                    spawner.firstSpawnTime = 0.7f;
                    spawner.secondSpawnTime = 0.6f;
                }
                else if (currScore > 70)
                {
                    spawner.firstSpawnTime = 0.6f;
                    spawner.secondSpawnTime = 0.5f;
                }

                if (currScore == 10) // change speed of the boxes after some progress
                    spawner.IncreaseDrag(3.5f);
                else if (currScore == 20)
                    spawner.IncreaseDrag(3f);
                else if (currScore == 30)
                    spawner.IncreaseDrag(2.8f);
                else if (currScore == 40)
                    spawner.IncreaseDrag(2.5f);
                else if (currScore == 50)
                    spawner.IncreaseDrag(2.2f);
                else if (currScore == 60)
                    spawner.IncreaseDrag(2f);
                else if (currScore == 70)
                    spawner.IncreaseDrag(1.8f);
                else if (currScore == 80)
                    spawner.IncreaseDrag(1.8f);
                else if (currScore == 90)
                    spawner.IncreaseDrag(1.8f);
                else if (currScore == 100)
                    spawner.IncreaseDrag(1.7f);
            }
        }

        private void DestroyAllBoxesOnScreen() // after match has ended destroy all the enemies
        {
            for (int i = 0; i < spawner.pooledObjectsWooden.Count; i++)
            {
                if (i < spawner.pooledObjectsMetal.Count && spawner.pooledObjectsMetal[i].activeInHierarchy)
                {
                    spawner.pooledObjectsMetal[i].GetComponent<EnemyBehaviour>().Disappear(false);
                }

                if (i < spawner.pooledObjectsPlatinum.Count && spawner.pooledObjectsPlatinum[i].activeInHierarchy)
                {
                    spawner.pooledObjectsPlatinum[i].GetComponent<EnemyBehaviour>().Disappear(false);
                }

                
                if (i < spawner.pooledObjectsSword.Count && spawner.pooledObjectsSword[i].activeInHierarchy)
                {
                    spawner.pooledObjectsSword[i].SetActive(false);
                }

                if (i < spawner.pooledObjectsHeart.Count && spawner.pooledObjectsHeart[i].activeInHierarchy)
                {
                    spawner.pooledObjectsHeart[i].SetActive(false);
                }

                if (i < spawner.pooledObjectsPowerUp.Count && spawner.pooledObjectsPowerUp[i].activeInHierarchy)
                {
                    spawner.pooledObjectsPowerUp[i].SetActive(false);
                }

                if (spawner.pooledObjectsWooden[i].activeInHierarchy)
                {
                    spawner.pooledObjectsWooden[i].GetComponent<EnemyBehaviour>().Disappear(false);
                }
            }
        }

        public void EndGame()
        {
            StopAllCoroutines();  // stop everything that is going on
            StartCoroutine(RestartLevel()); // restart level
        }

        private IEnumerator RestartLevel() // slow down time before death
        {
            spawner.CancelInvoke();  // stop spawning
            DestroyAllBoxesOnScreen(); // destroy everything on screen
            spawner.IncreaseDrag(4f); // restart drag
            Lose();

            yield return new WaitForSeconds(2f / slowness);

            deathMenu.gameObject.SetActive(true);
        }


        public void PauseButtonClicked()
        {   // on pause stop everything
            Paused = true;
            menu.settingsInGame.SetActive(true);
            menu.pauseImage.SetActive(false);
            menu.resumeImage.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ResumeButtonClicked()
        {
            // on resume reverse pause
            Paused = false;
            menu.settingsInGame.SetActive(false);
            menu.pauseImage.SetActive(true);
            menu.resumeImage.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}