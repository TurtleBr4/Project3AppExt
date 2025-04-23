using UnityEngine;
using UnityEngine.AI;
public class ChaseEnemy : Enemy
{
    public float detectionRange = 10f;
    [SerializeField]
    private GameObject spinTrail;
    public bool isAttacking = false;

    void Start()
    {
        Health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        spinTrail.SetActive(false);
    }

    void Update()
    {
        if (Player == null)
            return;
        if(Friendo == null)
        {
            return;
        }

        updateHealthDisplay();

        float distance = Vector3.Distance(transform.position, positionStateChecker().position);

        if (distance <= detectionRange)
        {
            anim.SetBool("playerInRange", true);
            agent.SetDestination(positionStateChecker().position);
            spinTrail.SetActive(true);
            isAttacking = true;
            Debug.Log("Enemy Spotted");
        }
        else
        {
            anim.SetBool("playerInRange", false);
            spinTrail.SetActive(false);
            isAttacking = false;
            Debug.Log("Lost the Enemy!");
        }

        if (Health <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("Kerblewy");
        }

    }
    
    public void doAttack()
    {
        if(positionStateChecker() == Friendo.transform)
        {
            Friendo.takeDamage(Damage);
        }
        else
        {
            Player.changeHeatlh(-Damage);
        }
        
        Debug.Log("Chaser has sent the attack signal!");
    }



}