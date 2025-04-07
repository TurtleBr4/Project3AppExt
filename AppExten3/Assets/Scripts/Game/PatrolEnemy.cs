using UnityEngine;

public class PatrolEnemy : Enemy
{
    public Transform[] patrolPoints; //assinged via inspector
    public float attackRange = 3f;
    public int attackCooldown = 500;
    private int currentWaypointIndex = 0;
    bool isPatrolling = true;
    bool noticedPlayer = false;

    void doPatrol(){
        if (patrolPoints.Length == 0)
            return;

        agent.SetDestination(patrolPoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Length;
        Debug.Log("Patrollin");
    }

    void doAttack(){
        Player.changeHeatlh(-Damage);
    }

    void Update()
    {
        if (Player == null)
            return;

        float distance = Vector3.Distance(agent.nextPosition, Player.transform.position);

        attackCooldown--;

        if (distance <= attackRange && attackCooldown <= 0)
        {
            doAttack();
            attackCooldown = 500;
        }
        else if (distance > attackRange && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            doPatrol();
        }
    }
}
