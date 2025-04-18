using UnityEngine;

public class HitboxSignal : MonoBehaviour
{
    public ChaseEnemy parentEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && parentEnemy.isAttacking)
        {
            parentEnemy.doAttack();
            Debug.Log("Chaser Attack!");
            
        }
    }
}
