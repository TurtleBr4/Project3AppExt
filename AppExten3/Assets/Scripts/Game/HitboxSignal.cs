using UnityEngine;

public class HitboxSignal : MonoBehaviour
{
    public ChaseEnemy parentEnemy;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTERED: " + other.name + " | TAG: " + other.tag + " | Attacking: " + parentEnemy.isAttacking);
        if (other.CompareTag("Player") && parentEnemy.isAttacking)
        {
            parentEnemy.doAttack();
            Debug.Log("Chaser Attack!");

            
        }
        else if (other.CompareTag("Helper") && parentEnemy.isAttacking)
        {
            parentEnemy.doAttack();
            Debug.Log("Chaser Attack!");
        }
    }
}
