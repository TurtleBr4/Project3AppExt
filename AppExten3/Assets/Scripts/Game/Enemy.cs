using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected int Health;
    protected int maxHealth = 100;
    [SerializeField]
    protected int Damage;
    protected float Speed;
    protected float attackSpeed;
    public bool isRangedAttacker;
    protected static Player Player; //reference stored so we dont have to constantly pull it
    protected Transform lastPlayerLocation;
    protected NavMeshAgent agent;
    [SerializeField]
    protected Animator anim;

    void Start()
    {
        Health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player").GetComponent<Player>();
        lastPlayerLocation = Player.gameObject.transform;
    }

    public void changeHealth(int amt)
    {
        Health += amt;
    }
    public int getHealth(){return Health;}
    public int getDamage(){return Damage;}

}
