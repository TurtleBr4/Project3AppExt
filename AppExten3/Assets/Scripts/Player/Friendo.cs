using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Friendo : MonoBehaviour
{
    
    private NavMeshAgent agent;
    private float lastShotTime;

    public int health;
    public int maxHealth = 100;
    
    public Transform player;
    public int moveSpeed;
    public int attackSpeed;
    public GameObject projectileAttack;
    public int attackDamage;
    public int shotCooldown;
    public Transform firePoint;

    public Sprite[] healthIcons;
    public Image healthDisplay;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        player = GameObject.Find("Player").transform;
        health = maxHealth;
        tag = "Helper";
    }

    private void Update()
    {
        if(player == null)
        {
            getPlayer();
            Debug.Log("How could this happen? We're smarter than this!");
            return; //handle any nonexistent player situation (if this were to happen we'd have bigger problems frankly)
        }

        agent.SetDestination(player.position);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance <= 5f && Time.time > lastShotTime + shotCooldown)
            {
                doAttack(enemy.transform);
                lastShotTime = Time.time;
            }
        }

        updateHealthDisplay();

        if(health <= 0)
        {
            keelOverAndDie();
        }
    }

    void doAttack(Transform target)
    {
        Projectile p;
        GameObject bullet = Instantiate(projectileAttack, firePoint.position, Quaternion.identity);
        p = bullet.GetComponent<Projectile>();
        p.damage = -attackDamage;
        p.shooterTag = tag;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            rb.linearVelocity = direction * attackSpeed;
        }
        Destroy(bullet, 5f);
    }


    void getPlayer()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
    }

    void updateHealthDisplay()
    {
        switch (health)
        {
            case > 84:
                healthDisplay.sprite = healthIcons[0];
                break;
            case > 68:
                healthDisplay.sprite = healthIcons[1];
                break;
            case > 52:
                healthDisplay.sprite = healthIcons[2];
                break;
            case > 36:
                healthDisplay.sprite = healthIcons[3];
                break;
            case > 20:
                healthDisplay.sprite = healthIcons[4];
                break;
            case > 0:
                healthDisplay.sprite = healthIcons[5];
                break;
            default:
                healthDisplay.sprite = healthIcons[6];
                break;
        }
    }

    void keelOverAndDie()
    {
        gameObject.SetActive(false);
    }

    

}
