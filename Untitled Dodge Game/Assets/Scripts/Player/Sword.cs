using System.Collections;
using Enemy;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class Sword : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject gfx;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private GameObject firstPart, secondPart, thirdPart;
        [SerializeField] private GameObject powerUp;
        [SerializeField] private PlayerMovement playerMovement;
        
        [Header("Stats")]
        public bool isActive;
        public bool canTake;
        public int hp;
        private int revivePoints;
        
        public bool isBuffed;

        public void Restart()
        {
            // on restart reset hp and other objects
            hp = 10;
            canTake = true;
            isActive = true;
            revivePoints = 0;
            powerUp.SetActive(false);
            isBuffed = false;
            firstPart.SetActive(true);
            secondPart.SetActive(true);
            thirdPart.SetActive(true);
            gfx.SetActive(true);
        }
        void Awake()
        {
            hp = 10;
            canTake = true;
            isActive = true;
            revivePoints = 0;
        }
        
        private void Update()
        {
            hpText.text = hp.ToString();
        }

        public void GetBuffed()
        {
            // if get special PowerUp, make sword unbreakable for 5 secs
            IEnumerator PowerUp()
            {
                yield return new WaitForSeconds(5f);
                powerUp.SetActive(false);
                isBuffed = false;
            }

            if (!isActive) return;
            
            if (isBuffed)
            {
                // if already buffed and you get another one, stop current and start new
                StopCoroutine(PowerUp());
                powerUp.SetActive(true);
                isBuffed = true;
                StartCoroutine(PowerUp());
            }
            else
            {
                powerUp.SetActive(true);
                isBuffed = true;
                StartCoroutine(PowerUp());
            }
        }

        public void AddRevive()
        {
            // when get reviver, add revivePoints and assemble sword
            revivePoints += 1;
            if (revivePoints == 1)
                firstPart.SetActive(true);
            else if(revivePoints == 2)
                secondPart.SetActive(true);
            else if (revivePoints == 3)
            {
                isActive = true;
                revivePoints = 0;
                hp = 3;
                thirdPart.SetActive(true);
                gfx.SetActive(true);
            }
        }
        
        public void TakeDamage(int damage, RaycastHit hit)
        {
            // if can't take damage, return, if can -hp and destroy object
            if (!isBuffed)
            {
                if (damage == hp)
                    isActive = false;
                
                if (!isActive)
                {
                    gfx.SetActive(false);
                    revivePoints = 0;
                    firstPart.SetActive(false);
                    secondPart.SetActive(false);
                    thirdPart.SetActive(false);
                }
                
                hp -= damage;
            }
            // StartCoroutine(Kill(hit));
            hit.transform.gameObject.GetComponent<EnemyBehaviour>().Disappear(true);
        }

        IEnumerator Kill(RaycastHit hit)
        {
            yield return new WaitForSeconds(0.05f);
            hit.transform.gameObject.GetComponent<EnemyBehaviour>().Disappear(true);
        }
    }
}
