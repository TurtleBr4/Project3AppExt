using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected int Health;
    protected int maxHealth;
    protected int Damage;
    protected float Speed;
    protected float attackSpeed;
    protected Player Player; //reference stored so we dont have to constantly pull it
    protected Transform lastPlayerLocation;
    protected NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player").GetComponent<Player>();
        lastPlayerLocation = Player.transform;
    }

    public int getHealth(){return Health;}
    public int getDamage(){return Damage;}

}
