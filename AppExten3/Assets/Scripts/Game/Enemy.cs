using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int Health;
    protected int maxHealth = 100;
    [SerializeField]
    protected int Damage;
    protected float Speed;
    protected float attackSpeed;
    public bool isRangedAttacker;
    protected static Player Player; //reference stored so we dont have to constantly pull it
    protected static Friendo Friendo;
    protected Transform lastPlayerLocation;
    protected NavMeshAgent agent;
    [SerializeField]
    protected Animator anim;

    void Start()
    {
        Health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player").GetComponent<Player>();
        Friendo = GameObject.Find("Friendo").GetComponent<Friendo>();
        lastPlayerLocation = Player.gameObject.transform;
    }

    public void changeHealth(int amt)
    {
        Health += amt;
    }

    private void Update()
    {
        
    }

    protected Transform positionStateChecker()
    {
        if(Friendo.gameObject.activeInHierarchy)
        {
            Debug.Log("friendo is the target");
            return Friendo.transform;
        }
        else
        {
            return Player.transform;
        }
    }

    public int getHealth(){return Health;}
    public int getDamage(){return Damage;}

}
