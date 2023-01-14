using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Health;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    // health
    [SerializeField] private GameObject blood;
    [SerializeField] private GameObject death;
    Health.Health _health;
    private float currHp;
    [SerializeField] private HealthBar _healthBar;

  
    
    // moving/following
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float offset;
    private Vector2 movement;

    // damaging
    [SerializeField] private bool inRange;
    [SerializeField] private float range;
    [SerializeField] private Animator _animator;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform attackPointUp;
    [SerializeField] private Transform attackPointDown;
    [SerializeField] private Transform attackPointLeft;
    [SerializeField] private Transform attackPointRight;

    [SerializeField] private float cooldown;

    [SerializeField] private bool canAttack = true;
    
    // collectibles
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject bounsPoint;
    
    // Score
    [SerializeField] private int deathScore;

    [SerializeField] private AudioManager audio;

    void Start()
    {
        Spawner.EnemyCounter++;
        currHp = 100f;
        _health = new Health.Health(100f);
        _healthBar.SetMaxHealth(100f);
        audio = FindObjectOfType<AudioManager>();
        if(!PlayerHealth.PlayerIsDead)
            player = FindObjectOfType<PlayerHealth>().gameObject.transform;
	if(ScoreManager.score > 1000)
	  moveSpeed = 3.5f;
    }
    
    private void Update()
    {
        if (player != null && currHp > 0)
        {
            inRange = (range >= Vector2.Distance(player.position, transform.position));

            Vector2 lookDir = player.position - transform.position;
            lookDir.Normalize();
            movement = lookDir;
            
            if (movement.y < 0 && Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
                attackPoint = attackPointDown;
            else if (movement.x > 0 && Mathf.Abs(movement.y) < Mathf.Abs(movement.x))
                attackPoint = attackPointRight;
            else if (movement.y > 0 && Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
                attackPoint = attackPointUp;
            else if (movement.x < 0 && Mathf.Abs(movement.y) < Mathf.Abs(movement.x))
                attackPoint = attackPointLeft;
            
            _animator.SetFloat("Horizontal", movement.x);
            _animator.SetFloat("Vertical", movement.y);
            _animator.SetFloat("Speed", movement.sqrMagnitude);

            if (inRange && canAttack)
            {
                movement = Vector2.zero;
                StartCoroutine(Attack());
            }
        }
    }

    private void FixedUpdate()
    {
        if (player != null && !inRange)
        {
            Vector2 lookDir = player.position - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - offset;
            
            Move(movement);
        }
    }
    
    // moving
    private void Move(Vector2 dir)
    {
        rb.MovePosition((Vector2) transform.position + (dir * moveSpeed * Time.fixedDeltaTime));
    }

    // attacking
    IEnumerator StartAttacking()
    {
        _animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    IEnumerator Attack()
    {
        
        canAttack = false;
        
        
        _animator.SetBool("Attacking", true);

        yield return new WaitForSeconds(0.4f);
        Collider2D[] playerHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        if (playerHit.Length > 0)
        {
            playerHit[0].GetComponent<PlayerHealth>().dealDamage(10f);
        }

        StartCoroutine(StartAttacking());
        
        yield return null;
    }

 
    
    // graphics

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    // collisions
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            movement = Vector2.zero;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            float damage;
            damage = (Shooting.currWeapon == Shooting.WeaponType.Pistol) ? 20 : 25;
            takeDamage(20f);
        }
    }
    
    // damaging
    public void takeDamage(float damage)
    {
        if (currHp > 0)
        {
            if (damage >= currHp)
            {
                _animator.SetTrigger("Died");
                ScoreManager.score += deathScore;
                int hp = Random.Range(1, 25);
                int bouns = Random.Range(1, 15);
                if (hp > 20)
                    Instantiate(heart, transform.position, Quaternion.identity);
                else if(bouns > 12)
                    Instantiate(bounsPoint, transform.position, Quaternion.identity);
                _healthBar.SetHealth(0);
                Spawner.EnemyCounter--;
                audio.PlayDieGoblin();
                movement = Vector2.zero;
                Instantiate(death, transform.position, Quaternion.identity);
                Destroy(gameObject, 0.8f);
            }

            Instantiate(blood, transform.position, Quaternion.identity);
            currHp -= damage;
            _healthBar.SetHealth(currHp);
            _health.setHealth(currHp);
            Debug.Log(currHp);
        }
        
    }
    
}
