using UnityEngine;

public class BossEnemy : Enemy
{
    public int phaseCounter = 0; //0 is inactive
    private Vector3 targetPosition;
    private bool isCharging = false;
    private bool isWaiting = false;
    public float delayBetweenCharges = 1f;
    public Transform spawn1;
    public Transform spawn2;
    public GameObject missilePrefab;
    public int missileDamage = 10;
    public int missileSpeed = 10;
 
    public void startFight()
    {
        phaseCounter = 1;
    }

    void Update()
    {

        switch (phaseCounter)
        {
            case 0://do literally nothing
                break;
            case 1:
                attackPlayer();
                break;
            case 2:
                delayBetweenCharges = .5f;
                attackPlayer();
                break;
        }

        if(Health < 50)
        {
            phaseCounter = 2;
        }
        
    }

    void attackPlayer()
    {
        if (!isCharging && !isWaiting)
        {
            startCharge();
        }

        if (isCharging)
        {
            agent.destination = targetPosition;

            //stop manually if we arrive (not relying on pathfinding precision)
            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance < 0.5f) 
            {
                stopCharge();
            }
        }
    }

    void startCharge()
    {
        Debug.Log("Boss Is Charging At You!");
        targetPosition = Player.transform.position;
        isCharging = true;
        agent.isStopped = false;
    }

    void stopCharge()
    {
        isCharging = false;
        agent.isStopped = true;
        int rnd = Random.Range(0, 10);
        if (rnd > 6)
        {
            shootMissiles();
        }
        else
        {
            StartCoroutine(WaitBeforeNextCharge());
        }
    }

    void shootMissiles()
    {
        Transform firePoint;
        GameObject missile;
        for(int i = 0; i < Random.Range(2, 5); i++) //shoot between 2 and 5 missiles
        {
            if(i % 2 == 0)
            {
                missile = Instantiate(missilePrefab, spawn1.position, spawn1.rotation);
                firePoint = spawn1;
            }
            else
            {
                missile = Instantiate(missilePrefab, spawn2.position, spawn2.rotation);
                firePoint = spawn2;
            }
            
            missile.GetComponent<Projectile>().damage = -missileDamage;
            Rigidbody rb = missile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = -firePoint.forward * missileSpeed;
            }
            Destroy(missile, 5f);


        }
        StartCoroutine(WaitBeforeNextCharge());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && isCharging)
        {
            stopCharge();
        }
    }

    System.Collections.IEnumerator WaitBeforeNextCharge()
    {
        isWaiting = true;
        yield return new WaitForSeconds(delayBetweenCharges);
        isWaiting = false;
    }
}
