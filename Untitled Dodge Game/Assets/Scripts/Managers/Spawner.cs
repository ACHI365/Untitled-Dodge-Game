using System.Collections.Generic;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class Spawner : MonoBehaviour
    {
        [Header("Enemies")]
        public GameObject woodenLog;
        public GameObject metal;
        public GameObject platinum;

        [Header("EnemyCount")]
        public int pooledAmountWooden;
        public int pooledAmountMetal;
        public int pooledAmountPlatinum;
        public int pooledAmountSword;
        public int pooledAmountHeart;
        public int pooledAmountPowerUp;

        [Header("EnemyLists")]
        public List<GameObject> pooledObjectsWooden;
        public List<GameObject> pooledObjectsMetal;
        public List<GameObject> pooledObjectsPlatinum;
        public List<GameObject> pooledObjectsSword;
        public List<GameObject> pooledObjectsHeart;
        public List<GameObject> pooledObjectsPowerUp;

        [Header("SpawnTimes")]
        public float firstSpawnTime;
        public float secondSpawnTime;
        public int counter;
        public int invokers;
        public int disInvokers;
        
        [Header("References")]
        [SerializeField] private GameObject heart, swordRevive, powerUp;
        [SerializeField] private Sword sword;
        private void Start()
        {
            // initialize lists
            pooledObjectsWooden = new List<GameObject>();
            pooledObjectsMetal = new List<GameObject>();
            pooledObjectsPlatinum = new List<GameObject>();
            pooledObjectsSword = new List<GameObject>();
            pooledObjectsHeart = new List<GameObject>();
            pooledObjectsPowerUp = new List<GameObject>();


            for (var i = 0; i < pooledAmountWooden; i++)
            {  // fill in lists with suitable objects
                if (i < pooledAmountMetal)
                {
                    var item = Instantiate(this.metal);
                    item.SetActive(false);
                    pooledObjectsMetal.Add(item);
                }

                if (i < pooledAmountPlatinum)
                {
                    var item = Instantiate(this.platinum);
                    item.SetActive(false);
                    pooledObjectsPlatinum.Add(item);
                }
                if (i < pooledAmountSword)
                {
                    var item = Instantiate(swordRevive);
                    item.SetActive(false);
                    pooledObjectsSword.Add(item);
                }
                if (i < pooledAmountHeart)
                {
                    var item = Instantiate(heart);
                    item.SetActive(false);
                    pooledObjectsHeart.Add(item);
                }

                if (i < pooledAmountPowerUp)
                {
                    var item = Instantiate(powerUp);
                    item.SetActive(false);
                    pooledObjectsPowerUp.Add(item);
                }
                var wood = Instantiate(woodenLog);
                wood.SetActive(false);
                pooledObjectsWooden.Add(wood);
            }
        }

        void Update()
        {
            // + counter for every 2 block spawned
            if (counter % 2 == 0)
                ScoreManager.Score = counter / 2;
        }

        // make 2 separate generators in order to make sure everything goes smooth
        private void GenerateRandomBoxes()
        {
            // make chance with random numbers
            int num = Random.Range(0, 6);
            int powerUpChance = Random.Range(0, 10);
            float[] arr = new float[] {0f, 2f, 3f, 4.2f, -0.2f, -1f, -3.5f, -4.9f}; // set positions to spawn
            if (powerUpChance > 8) // spawn powerUp
            {
                int num3 = Random.Range(0, 7);
                Generator(arr[num3], true);
            }
            else if (num < 3) // if no powerUp, spawn block
            {
                float[] array = new float[] {0f, 2f, 3f, 4.2f};
                int num2 = Random.Range(0, array.Length);
                Generator(array[num2], false);
            }
            else
            {
                float[] array2 = new float[] {-0.2f, -1f, -3.5f, -4.9f};
                int num3 = Random.Range(0, array2.Length);
                Generator(array2[num3], false);
            }
        }

        private void GenerateRandomBoxes2()
        {
            int num = Random.Range(0, 5);
            if (num < 3)
            {
                float[] array = new float[] {-1.8f, -2f, -2.9f, -4f};
                int num2 = Random.Range(0, array.Length);
                Generator(array[num2], false);
            }
            else
            {
                float[] array2 = new float[] {0.7f, 1f, 3.6f, 4.9f};
                int num3 = Random.Range(0, array2.Length);
                Generator(array2[num3], false);
            }
        }

        
        // invoke generate box and repeat
        public void InvokeBoxGenerator()
        {
            invokers++;
            InvokeRepeating(nameof(GenerateRandomBoxes), 1f, firstSpawnTime);
            InvokeRepeating(nameof(GenerateRandomBoxes2), 1f, secondSpawnTime);
        }

        // cancel spawning
        public new void CancelInvoke()
        {
            disInvokers++;
            CancelInvoke(nameof(GenerateRandomBoxes));
            CancelInvoke(nameof(GenerateRandomBoxes2));
        }
        
        // generator which chooses object to spawn
        private void Generator(float rand, bool isPowerUp)
        {
            if (isPowerUp) // if powerUp should spawn
            {
                if (sword.isActive) // if sword is active don't spawn reviver
                {
                    int range = Random.Range(0, 5);
                    if (range < 2)
                        ActivatePooledObject(pooledObjectsPowerUp, powerUp, rand);
                    else 
                        ActivatePooledObject(pooledObjectsHeart, heart, rand);
                }
                else
                {
                    // check to be only one active of same kind
                    bool another = false;
                    foreach (var t in pooledObjectsSword)
                        if(t.activeInHierarchy)
                            another = true;
                    if (!another)
                        ActivatePooledObject(pooledObjectsSword, swordRevive, rand);
                }
            }

            else
            {
                // choose probability and spawn object
                int metalProbability = Random.Range(0, 10);
                int platinumProbability = Random.Range(0, 50);

                if (metalProbability is 5 or 6)
                {
                    ActivatePooledObject(pooledObjectsMetal, metal, rand);
                    counter++;
                }
                else if (platinumProbability is > 32 and < 36)
                {
                    ActivatePooledObject(pooledObjectsPlatinum, platinum, rand);
                    counter++;
                }
                else
                {
                    ActivatePooledObject(pooledObjectsWooden, woodenLog, rand);
                    counter++;
                }
            }
            
        }

        // activate object based on its nature and list
        private void ActivatePooledObject(List<GameObject> list, GameObject spawn, float pos)
        {
            GameObject pooledObject = GetPooledObject(list, spawn);
            pooledObject.transform.position = new Vector3(pos, 5f, -6f);
            pooledObject.SetActive(true);
            pooledObject.transform.gameObject.SetActive(true);
        }

        // get pooled object, make new method in order to avoid error in case of lack the boxes
        private GameObject GetPooledObject(List<GameObject> pool, GameObject box)
        {
            foreach (var t in pool)
            {
                if (!t.activeInHierarchy)
                {
                    return t;
                }
            }

            GameObject log = Instantiate(box);
            log.SetActive(false);
            pool.Add(log);
            return log;
        }
        
        // change drag to speedUp boxes

        public void IncreaseDrag(float drag)
        {
            for (int i = 0; i < pooledObjectsWooden.Count; i++)
            {
                if (i < pooledAmountMetal)
                    pooledObjectsMetal[i].GetComponent<Rigidbody>().drag = drag;
                if (i < pooledAmountPlatinum)
                    pooledObjectsPlatinum[i].GetComponent<Rigidbody>().drag = drag;
                pooledObjectsWooden[i].GetComponent<Rigidbody>().drag = drag;
            }
        }
    }
}