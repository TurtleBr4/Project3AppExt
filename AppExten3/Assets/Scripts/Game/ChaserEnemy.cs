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