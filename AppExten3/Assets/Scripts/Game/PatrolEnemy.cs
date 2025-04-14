using Unity.VisualScripting;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    public Transform[] patrolPoints; //assinged via inspector
    public float attackRange = 3f;
    public int attackCooldown = 500;
    private int currentWaypointIndex = 0;
    private float followRange = 6f;

    private int currentState = 0; //0 patrol, 1 chase
    bool isPatrolling = true;
    bool noticedPlayer = false;

    [SerializeField] private Transform thingThatLooksAtPlayer;

    void doPatrol(){
        if (patrolPoints.Length == 0)
            return;

        agent.SetDestination(patrolPoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Length;
        Debug.Log("Patrollin");
    }

    void doAttack(float dist){

        currentState = 1;

        if(!isRangedAttacker){
            agent.SetDestination(Player.transform.position); //break the patrol routine, follow the player

            if(dist <= attackRange / 2) //within attack range, actually attack
            { 
                Player.changeHeatlh(-Damage);
                attackCooldown = 500;
            }
        }
        else{

            thingThatLooksAtPlayer.LookAt(Player.transform);
        }
        
    }

    void Update()
    {
        if (Player == null)
            return;

        float distance = Vector3.Distance(agent.nextPosition, Player.transform.position);

        attackCooldown--;

        if (distance <= attackRange && attackCooldown <= 0 && currentState != 1) //player is in range and not already attacking
        {
            doAttack(distance);
            attackCooldown = 500;
        }
        else if (distance > attackRange && !agent.pathPending && agent.remainingDistance < 0.5f) //player isnt near
        {
            
            doPatrol();
        }
    }
}
