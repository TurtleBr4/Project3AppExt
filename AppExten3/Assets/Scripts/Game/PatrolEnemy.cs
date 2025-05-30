using Unity.VisualScripting;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    public Transform[] patrolPoints; //assinged via inspector
    public float attackRange = 3f;
    public int attackCooldown = 100;
    private int currentWaypointIndex = 0;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform firePoint2;
    public int projectileSpeed;


    [SerializeField] private Transform thingThatLooksAtPlayer;


    void doPatrol(){
        if (patrolPoints.Length == 0)
            return;

        agent.SetDestination(patrolPoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Length;
        Debug.Log("Patrollin");

    }

    void doAttack(float dist){

        agent.isStopped = true; ;
        gameObject.transform.LookAt(positionStateChecker());

        int rnd = Random.Range(0, 2);
        GameObject projectile;
        Rigidbody rb;
        Projectile pComp;
        switch (rnd)
        {
            case 0:
                projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                pComp = projectile.GetComponent<Projectile>();
                pComp.damage = Damage;
                pComp.shooterTag = tag;
                rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = firePoint.forward * projectileSpeed;
                }
                Destroy(projectile, 5f);
                attackCooldown = 500;
                break;
            case 1:
                projectile = Instantiate(projectilePrefab, firePoint2.position, Quaternion.identity);
                pComp = projectile.GetComponent<Projectile>();
                pComp.damage = Damage;
                pComp.shooterTag = tag;
                rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = firePoint2.forward * projectileSpeed;
                }
                Destroy(projectile, 5f);
                attackCooldown = 500;
                break;
        }
        

    }

    void Update()
    {
        if (Player == null)
            return;

        float distance = Vector3.Distance(agent.nextPosition, positionStateChecker().position);

        updateHealthDisplay();

        if (attackCooldown != 0)
        {
            attackCooldown--;
        }

        if (distance <= attackRange && attackCooldown <= 0) //player is in range and not already attacking
        {
            doAttack(distance);
        }
        else if (distance > attackRange) //player isnt near
        {
            agent.isStopped = false;
            doPatrol();
        }

        if (Health <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("Kerblewy");
        }
    }
}
