using System.Collections;
using Managers;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [Header("availability")]
        [SerializeField] private new string tag; // tag to acknowledge current prefab
        private bool slice = true; // make sure to slice only once
        
        [Header("instantiatable")]
        public GameObject particle;     // slice and instantiate particle on death
        public GameObject slicePrefab;
        
        [Header("references")]
        public UltimateAbility ultimateAbility;
        public Spawner spawner;

        private void Start()
        {
            ultimateAbility = FindObjectOfType<UltimateAbility>();  // since it is prefab, find ultimate ability when start
            spawner = FindObjectOfType<Spawner>();
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground") && gameObject.activeSelf) // if on ground or another box, stay for a while
                StartCoroutine(Stay());
            else if (collision.gameObject.CompareTag("Player") &&
                     !gameObject.CompareTag("Ground") && gameObject.activeSelf) // if hit an enemy, disappear immediately
            {
                Disappear(false);
            }
        }

        public void Disappear(bool destroyed)
        {
            StopCoroutine(Stay());  // stop coroutine so it won't interrupt
            if(destroyed) // determined if it died, or was killed
                switch (tag)
                {
                    case "Metal":
                        spawner.counter += 3;
                        break;
                    case "Platinum":
                        spawner.counter += 5;
                        break;
                    case "WoodenLog":
                        spawner.counter++;
                        break;
                }
            Instantiate(particle, transform.position, Quaternion.identity); // create death particle
            gameObject.tag = tag;  // restore tag
        
            
            slice = true; // let it slice again
            transform.gameObject.SetActive(false); // make invisible
            gameObject.SetActive(false);
        }

        private IEnumerator Stay()
        {
            if (!CompareTag("Platinum")) // if it is not platinum, be safe
            {
                gameObject.tag = "Ground"; // change tag to become safe
                yield return new WaitForSeconds(2f);
                gameObject.tag = tag; // return tag for later spawning
                Disappear(false);
            }
            else
            {
                // if it is platinum spikes should kill as well, so don't change tag
                yield return new WaitForSeconds(2f);
                Disappear(false);
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Blade") && slice) // if is can be sliced and blade touches act following
            {
                StopCoroutine(Stay()); // stop coroutine stay to not interfere
                Vector3 direction = (other.transform.position - transform.position).normalized; // get direction from where it is being hit
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // get angle for z coordinate
                Quaternion rotation = Quaternion.Euler(0f,0f, angle);
                Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.9f); // get position for slice effect
                
                ultimateAbility.AddToList(
                    new Nme(gameObject, slicePrefab, newPos, rotation)); // add new Nme object to ultimate ability,
                                                                                    // with position current block, slicing prefab, its position and rotation
                slice = false; // cannot be sliced again
            }
        }
    }
}