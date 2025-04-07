using UnityEngine;

public class PatrolEnemy : Enemy
{
    public Transform[] patrolPoints; //assinged via inspector
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
        //Player.changeHealth(-Damage);
    }

    void Update()
    {
        doPatrol();
    }
}
