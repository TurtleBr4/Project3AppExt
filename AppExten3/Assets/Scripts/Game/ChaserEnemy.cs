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

        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance <= detectionRange)
        {
            anim.SetBool("playerInRange", true);
            agent.SetDestination(Player.transform.position);
            spinTrail.SetActive(true);
            isAttacking = true;
        }
        else
        {
            anim.SetBool("playerInRange", false);
            spinTrail.SetActive(false);
            isAttacking = false;
        }

    }

    
    public void doAttack()
    {
        Player.changeHeatlh(-Damage);
        Debug.Log("Chaser has sent the attack signal!");
    }



}