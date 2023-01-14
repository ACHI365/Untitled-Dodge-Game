using System.Collections;
using Managers;
using Unity.VisualScripting;

namespace Player
{
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float offset;
        public float moveSpeed;
        private float movement;
        private float direction;
        private Vector3 temp = Vector3.zero;
        private bool firstTouch;

        [Header("Jumping")] private bool jump;
        public float jumpForce;
        public bool grounded;
        private bool jumped;
        public bool ultimate;
        
        float _timer = 0f;
        float _maxTime = 0.2f;
        bool _timerStarted = false;

        [Header("Player Parameters")] [SerializeField]
        private float playerHeight;
        [SerializeField] private float rigControl;
        
        [Header("References")]
        [SerializeField] private Sword sword;
        private Rigidbody rb;
        private Touch touch;
        public bool gameIsStarted;
        [SerializeField] private Animator animator;
        public GameObject body;

        [Header("Sword")] [SerializeField] private float distanceToCut;
        public bool canCut;
        private RaycastHit hit;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            transform.position = new Vector3(0, -0.2755553f, -6);
        }

        void Update()
        {
            var newPos = new Vector3(transform.position.x, transform.position.y + rigControl, transform.position.z);
            // check if is grounded or has something above
            grounded = Physics.Raycast(transform.position, Vector3.down,
                playerHeight * 0.5f + 0.2f); // check ground with a raycast
            
            if (!ultimate)
            {
                canCut = (Physics.Raycast(transform.position, Vector3.up, out hit, playerHeight * 0.5f + distanceToCut)
                          || Physics.Raycast(
                              new Vector3(transform.position.x + offset, transform.position.y, transform.position.z),
                              Vector3.up, out hit, playerHeight * 0.5f + distanceToCut)
                          || Physics.Raycast(
                              new Vector3(transform.position.x - offset, transform.position.y, transform.position.z),
                              Vector3.up, out hit,
                              playerHeight * 0.5f + distanceToCut)); // check incoming box with a raycast

                canCut = canCut && hit.transform.gameObject.GetComponent<Rigidbody>().velocity.y != 0;
            }
           
            if (grounded)
                jumped = false;

            
            if (gameIsStarted && !ManageGame.Paused)
            {
                CutIfPossible();
                GetInputs();
            }
        }

        private void CutIfPossible()
        {
            if (canCut && sword.isActive)
            {
                if (hit.transform.gameObject.CompareTag("WoodenLog") && (sword.hp > 0 || sword.isBuffed))
                {
                    sword.TakeDamage(1, hit);
                    Attack();

                }
                else if (hit.transform.gameObject.CompareTag("Metal") && (sword.hp > 4 || sword.isBuffed))
                {
                    sword.TakeDamage(5, hit);
                    Attack();

                }
                else if (hit.transform.gameObject.CompareTag("Platinum") && (sword.hp > 9 || sword.isBuffed))
                {
                    sword.TakeDamage(10, hit);
                    Attack();

                }
                
            }
        }
        
        void Attack()
        {
            animator.SetTrigger("Attack");
            animator.SetBool("ShouldAttack", true);
            StartCoroutine(DisableAttack());
        }
        
        IEnumerator DisableAttack()
        {
            yield return new WaitForSeconds(0.28f);
            animator.SetBool("ShouldAttack", false);
        }

        private void FixedUpdate()
        {   // if is not paused and game is started move and jump
            if (gameIsStarted && !ManageGame.Paused)
            {
                animator.SetBool("inAir", !grounded);
                Move();
            }

            if (gameIsStarted && !ManageGame.Paused && PlayerPrefs.GetInt("controller") == 60)
            {
                if (jump && grounded)
                    Jump();
            }
        }

        void GetInputs()
        {
            // based on controller get inputs
            if (PlayerPrefs.GetInt("controller") == 60)
            {
                // if gyroscope move  based on acceleration and jump on click
                float x = Input.acceleration.x;
                direction = (x > 0f) ? 1f : -1f;
                movement = direction * moveSpeed * 10 * Time.fixedDeltaTime; // make movement

                if (Input.touchCount > 0 && firstTouch && !jumped)
                {
                    jump = true;
                }
            }


            else
            {
                if(_timerStarted)
                    _timer += Time.deltaTime;
                if (Input.GetMouseButtonDown(0))
                {
                    _timerStarted = true;
                }
                // if on touchControl, move on slide and jump on release
                if (Input.touchCount > 0 && firstTouch) // if there is touch input
                {
                    touch = Input.GetTouch(0); // take first input
                    jump = true; // make jump possible
                    if (touch.phase == TouchPhase.Moved) // if moved 
                    {
                        direction = (touch.deltaPosition.x > offset) ? 1f :
                            (touch.deltaPosition.x < -offset) ? -1f : 0f; // see the direction
                        movement = direction * moveSpeed * 10 * Time.fixedDeltaTime; // make movement
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (_timer < _maxTime && jump && grounded && firstTouch)
                        Jump();
                    else
                    {
                        direction = 0; // if not jumping and there is no touch, stop moving
                        movement = direction * moveSpeed * 10 * Time.fixedDeltaTime;
                    }
                    _timerStarted = false;
                    _timer = 0f;
                }
            }
        }

        void Move()
        {
            
            // change velocity to move and according to direction, change rotation
            Vector3 targetVelocity = new Vector3(movement * 5f, rb.velocity.y, rb.velocity.z);

            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref temp, 0.05f);
            float directionChanger = (Mathf.Abs(rb.velocity.x) > Mathf.Abs(direction)) ? Mathf.Abs(direction) : Mathf.Abs(rb.velocity.x); 
            animator.SetFloat("direction", directionChanger);
            
            if (direction < 0) // according to direction, rotate
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    90,
                    transform.eulerAngles.z
                );
            else if (direction > 0)
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    -90,
                    transform.eulerAngles.z
                );
            else
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    0,
                    transform.eulerAngles.z
                );
        }

        void Jump()
        {
            jumped = true;
            jump = false;
            rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // on collision die
            if (collision.gameObject.CompareTag("WoodenLog") ||
                collision.gameObject.CompareTag("Metal") ||
                collision.gameObject.CompareTag("Platinum")) // if hit without sword, die
            {
                FindObjectOfType<ManageGame>().EndGame();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
            // based om collision, either die or powerUp or return blade
            if (other.gameObject.CompareTag("Revive") && !sword.isActive)
            {
                sword.AddRevive();
                other.transform.gameObject.SetActive(false);
                other.gameObject.SetActive(false);
            }

            if (other.gameObject.CompareTag("PowerUp"))
            {
                if (sword.hp < 10 && sword.isActive)
                {
                    sword.hp++;
                }

                other.transform.gameObject.SetActive(false);
                other.gameObject.SetActive(false);
            }

            if (other.gameObject.CompareTag("Golden"))
            {
                if (sword.isActive)
                {
                    sword.GetBuffed();
                }

                other.transform.gameObject.SetActive(false);
                other.gameObject.SetActive(false);
            }

            if (other.gameObject.CompareTag("Spike"))
            {
                FindObjectOfType<ManageGame>().EndGame();
            }
        }

        private void OnDrawGizmos()
        {
            var newPos = new Vector3(transform.position.x, transform.position.y + rigControl, transform.position.z);

            Gizmos.DrawLine(transform.position,
                new Vector3(transform.position.x, newPos.y + playerHeight * 0.5f + distanceToCut,
                    transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x + offset, newPos.y, transform.position.z),
                new Vector3(transform.position.x + offset, newPos.y + playerHeight * 0.5f + distanceToCut,
                    transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x - offset, newPos.y, transform.position.z),
                new Vector3(transform.position.x - offset, newPos.y + playerHeight * 0.5f + distanceToCut,
                    transform.position.z));
        }


        public void StopMove()
        {
            firstTouch = false;
        }

        public void StartMove()
        {
            StartCoroutine(MovePlayerActive());
        }

        private IEnumerator MovePlayerActive()
        {
            // let player move after few secs to ensure smoothness 
            yield return new WaitForSeconds(0.2f);
            firstTouch = true;
            jump = false;
        }

        public void Restart()
        {  // restart its transform and make sure it does not move
            transform.position = new Vector3(0, 0, -6);
            firstTouch = false;
            sword.Restart();
        }
    }
}