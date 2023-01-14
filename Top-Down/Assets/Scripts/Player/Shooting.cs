using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Shooting : MonoBehaviour
{
    // bullet
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    public TextMeshProUGUI AmmoText;

    // shootAvailability
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;
    private bool canShoot = true;

    // animator
    Animator _animator;
    [SerializeField] AnimatorOverrideController pistol;
    [SerializeField] AnimatorOverrideController AK;

    [SerializeField] private float instantiateDelay;
    [SerializeField] private float MeleeDelay;
    private bool ismelee;

    // bullets
    private int bulletCount = 0;
    private int weaponBullets;
    
    // melee
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    
    // Weapons
    public enum WeaponType {AK, Pistol};

    public static WeaponType currWeapon;

    public float reloadTime;
    private bool isReloading;


    [SerializeField] private AudioManager audio;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        currWeapon = WeaponType.Pistol;
        SetAnimations(pistol);
    }
    
    private void SetAnimations(AnimatorOverrideController overrid)
    {
            _animator.runtimeAnimatorController = overrid;
    }

    void Update()
    {
        fireRate = (currWeapon == WeaponType.Pistol) ? 2 : 5;
        weaponBullets = (currWeapon == WeaponType.Pistol) ? 15 : 30;
        AmmoText.text = (weaponBullets - bulletCount).ToString();

        if (ScoreManager.score >= 1000)
        {
            currWeapon = WeaponType.AK;
            SetAnimations(AK);
        }
        if (Input.GetKeyDown(KeyCode.E) && !ismelee)
        {
            Debug.Log("noice");
            ismelee = true;
            _animator.SetTrigger("Melee");
            StartCoroutine(Melee());
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                if (canShoot && !PauseMenu.GameIsPaused && bulletCount != weaponBullets && !ismelee && !isReloading)
                {
                    _animator.SetTrigger("Shot");
                    StartCoroutine(Shoot());
                }
                if (bulletCount >= weaponBullets && !isReloading)
                {
                    StartCoroutine(Reload());
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && !ismelee && !isReloading)
            {
                StartCoroutine(Reload());
            }
        }
        
        
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        audio.PlayShot();
        GameObject bullet = Instantiate(this.bullet, shootPoint.position, shootPoint.rotation);
        bulletCount++;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(instantiateDelay);
        if(rb!= null)
            rb.AddForce(shootPoint.up * bulletSpeed, ForceMode2D.Impulse);
        
        

        StartCoroutine(shootingDelay());
        yield return null;
    }

    IEnumerator shootingDelay()
    {
        float delay = 1f / fireRate;
        yield return new WaitForSeconds(delay);
        // _animator.SetBool("isShooting", false);
        canShoot = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        _animator.SetTrigger("Reload_Pistol");
        audio.PlayReload();
        yield return new WaitForSeconds(reloadTime);
        bulletCount = 0;
        isReloading = false;
    }

    IEnumerator Melee()
    {
        _animator.SetTrigger("Attack");
        
        yield return new WaitForSeconds(MeleeDelay);
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy  in enemyHit)
        {
            enemy.GetComponent<Enemy>().takeDamage(50f);
        }
        
        yield return new WaitForSeconds(MeleeDelay);
        
        ismelee = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
}