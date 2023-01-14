using System.Collections;
using System.Collections.Generic;
using Enemy;
using Managers;
using UnityEngine;

namespace Player
{
    public class UltimateAbility : MonoBehaviour
    {
        [Header("References")]
        public GameObject blackScreen;
        public Spawner spawner;
        public GameObject blade;
        public PlayerMovement PlayerMovement;
        
        [Header("SetStats")]
        public float slowness;
        public float threshold = 30;
        
        
        private bool can = true;
        private bool coding; //coding and blockchain

        private List<Nme> toDestroy = new List<Nme>();
        private List<GameObject> prefabs = new List<GameObject>();
        private List<GameObject> enemies = new List<GameObject>();


        private void RemovePrefabs()
        {
            // remove everything from lists
            foreach (var sprite in prefabs)
            {
                Destroy(sprite);
            }

            foreach (var enemy in enemies)
            {
                enemy.GetComponent<EnemyBehaviour>().Disappear(true);
            }

        }

        public void AddToList(Nme enemy)
        { 
            // add enemy to list from external class
            toDestroy.Add(enemy);
        }

        void Update()
        {
            if (ScoreManager.Score > threshold && can)
            {
                StartCoroutine(SlowTime());
            }
        }

        IEnumerator SlowTime()
        {
            PlayerMovement.StopMove();
            PlayerMovement.ultimate = true;
            // activate blade, slow time and let player slice for 5 secs
            blade.SetActive(true);
            can = false;
            spawner.CancelInvoke();
            Time.timeScale = 1f / slowness;
            Time.fixedDeltaTime = Time.fixedDeltaTime / slowness;

            yield return new WaitForSeconds(5f / slowness);

            // pause game to stop player and make slices
            ManageGame.Paused = true;
            blade.SetActive(false);
            blackScreen.SetActive(true);
            yield return StartCoroutine(WaitForRealSeconds(toDestroy.Count/1.5f + 0.5f));
            if (coding)
            {
                PlayerMovement.ultimate = false;
                PlayerMovement.StartMove();
                RemovePrefabs();
                RemovePrefabs();

                blackScreen.SetActive(false);
                // initialize lists to become empty on next ultimate ability
                toDestroy = new List<Nme>();
                prefabs = new List<GameObject>();
                enemies = new List<GameObject>();
                
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.fixedDeltaTime * slowness;

                // invoke box generator since it was canceled earlier
                spawner.InvokeBoxGenerator();
                ManageGame.Paused = false;
                threshold += 30;
                can = true;
                coding = false;
            }
        }

        IEnumerator WaitForRealSeconds(float seconds)
        {
            
            // wait for real seconds and activate slices along the way
            float startTime = Time.realtimeSinceStartup;
            int counter = 0;
            while (Time.realtimeSinceStartup - startTime < seconds)
            {
                if (counter % 30 == 0)
                {
                    if (toDestroy.Count > 0)
                    {
                        var temp = toDestroy[0];
                        toDestroy.RemoveAt(0);
                        prefabs.Add(Instantiate(temp.SlicePrefab, temp.Position, temp.Rotation));
                        enemies.Add(temp.Enemy);
                    }
                }
                if(toDestroy.Count == 0)
                {
                    Time.timeScale = 0f;
                }

                counter++;

                yield return null;
            }

            coding = true;
        }
    }
}