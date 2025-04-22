using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Friendo : MonoBehaviour
{
    
    private NavMeshAgent agent;
    private float lastShotTime;

    
    public Transform player;
    public int moveSpeed;
    public int attackSpeed;
    public GameObject projectileAttack;
    public int attackDamage;
    public int shotCooldown;
    public Transform firePoint;

    private int stupidCounterThatIHaveToAddToGetThisThingToWork = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        player = GameObject.Find("Player").transform;
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
    }

    void doAttack(Transform target)
    {
        GameObject bullet = Instantiate(projectileAttack, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().damage = -attackDamage;
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

    

}
