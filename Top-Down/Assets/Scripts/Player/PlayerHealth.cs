using System;
using System.Security.Cryptography;
using Health;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Health.Health _health;
    private float currHp;
    public static bool PlayerIsDead = false;

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private ScoreManager score;

    [SerializeField] private AudioManager audio;
    void Start()
    {
        currHp = 100f;
        _health = new Health.Health(100f);
        _healthBar.SetMaxHealth(100f);
        PlayerIsDead = false;
    }

    public void dealDamage(float damage)
    {
        if (damage >= currHp)
        {
            // Instantiate(death, transform.position, Quaternion.identity);
            PlayerIsDead = true;
            _healthBar.SetHealth(0);
            score.End();
            audio.PlayDie();
            Destroy(gameObject);
        }
        // Instantiate(blood, transform.position, Quaternion.identity);
        currHp -= damage;
        _health.setHealth(currHp);
        _healthBar.SetHealth(currHp);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            dealDamage(10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Heart"))
        {
            currHp = (currHp < 90f) ? currHp + 10f : 100f;
            _health.setHealth(currHp);
            _healthBar.SetHealth(currHp);
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Bonus"))
        {
            ScoreManager.score += 10;
            Destroy(col.gameObject);
        }
    }
}
