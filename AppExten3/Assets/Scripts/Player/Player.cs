using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    //Setup
    public Camera mainCamera;
    public Transform playerHead;
    private CharacterController controller;
    public DialogueManager yapper;
    public int Health;
    public int maxHealth;
    public Transform playerModel;
    public bool isDead = false;

    //Movement
    public float moveSpeed = 10f;
    public float runSpeed;
    public float gravity = -9.81f;
    public bool isRunning = false;
    private Vector3 velocity;
    private Vector3 movementInput;
    public bool isFrozen = false;


    //Animation and Attacking
    private Animator anim;
    public bool isMoving = false;
    public bool isAttacking = false;
    public int currentItemId = -1; //-1 is no item 
    private Quaternion lastRotation;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public int projectileSpeed = 10;
    public LayerMask enemyLayer;
    private int range;
    private int radius;
    public GameObject lazerPrefab;

    //audio
    public AudioClip playerShoot;
    private AudioSource audioSource;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        enemyLayer = LayerMask.GetMask("Enemy");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        runSpeed = 2 * moveSpeed;
        Health = maxHealth;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = playerShoot;
        audioSource.loop = false;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen) { goMove(); }
        Debug.Log(Health);
        lastRotation = playerModel.transform.rotation;
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeScale > .5f)
        {
            attackNow();
        }

        if(Health <= 0 ){
            gameOver();
        }
    }

    private void LateUpdate()
    {
        RotateToMouse();
    }
    void RotateToMouse()
    {
        // Find the mouse position in the world
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, playerHead.position); // Plane at object's height

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - playerHead.position).normalized;

            // Rotate towards the mouse
            Quaternion lookRotation = Quaternion.LookRotation(-direction, Vector3.up);
            playerHead.rotation = Quaternion.Slerp(playerHead.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    void goMove()
    {
        anim.SetInteger("Direction", 5);
        isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : moveSpeed;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            movementInput = Vector3.zero;
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
            movementInput = (transform.right * horizontal + transform.forward * vertical).normalized * speed;
        }

        if(horizontal > 0)
        {
            anim.SetInteger("Direction", 2);
            anim.SetInteger("LastDirection", 2);
        }
        else if (horizontal < 0)
        {
            anim.SetInteger("Direction", 1);
            anim.SetInteger("LastDirection", 1);
        }
        if (vertical > 0)
        {
            anim.SetInteger("Direction", 0);
            anim.SetInteger("LastDirection", 0);
        }
        else if (vertical < 0)
        {
            anim.SetInteger("Direction", 3);
            anim.SetInteger("LastDirection", 3);
        } //not my proudest code block, no sir

        velocity.y += gravity * Time.deltaTime;
        Vector3 move = movementInput * Time.deltaTime;
        move.y = velocity.y * Time.deltaTime;
        controller.Move(move);
        
        

    }

    void attackNow()
    {
        isAttacking = true;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Assume ground plane is at y = firePoint.y
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, firePoint.position.y, 0));

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - firePoint.position);
            direction.y = 0; // Ignore vertical component
            direction.Normalize();

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
            Projectile p = projectile.GetComponent<Projectile>();
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            p.shooterTag = tag;
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
            Destroy(projectile, 5f);
        }
        anim.SetTrigger("attack");
        audioSource.Play();
        
    }

    public void specialAttackNow(int damage, int id)
    {
        Debug.Log("special attack!");
        //for weapon items, blast wave, 
        switch (id){
            case 6://demon core
                performSphereCastAttack(40, 40, -damage);
                changeHeatlh(-20);
                Debug.Log("DEMON CORE!!");
                break;
            case 7://lazer pew pew
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                // Assume ground plane is at y = firePoint.y
                Plane groundPlane = new Plane(Vector3.up, new Vector3(0, firePoint.position.y, 0));

                if (groundPlane.Raycast(ray, out float distance))
                {
                    Vector3 targetPoint = ray.GetPoint(distance);
                    Vector3 direction = (targetPoint - firePoint.position);
                    direction.y = 0; // Ignore vertical component
                    direction.Normalize();

                    GameObject projectile = Instantiate(lazerPrefab, firePoint.position, Quaternion.LookRotation(-direction));
                    Projectile p = projectile.GetComponent<Projectile>();
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();
                    p.damage = damage;
                    p.shooterTag = tag;
                    if (rb != null)
                    {
                        rb.linearVelocity = -direction * 0;
                    }
                if(Input.GetKeyUp(KeyCode.Mouse1)){
                    Destroy(projectile);
                }
                Destroy(projectile, 1f);
                }
                break;
            case 8:
            performSphereCastAttack(50, 50, -damage);
                changeHeatlh(-99);
                Debug.Log("NVKED");
                break;
            default:
                break;
        }
    }

    public void performSphereCastAttack(int rad, int rang, int damage)
    {
        range = rang;
        radius = rad;

        RaycastHit[] hits = Physics.SphereCastAll(
            origin: transform.position,
            radius: radius,
            direction: transform.forward,
            maxDistance: range,
            layerMask: enemyLayer
        );

        foreach (RaycastHit hit in hits)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.changeHealth(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * range);
        Gizmos.DrawWireSphere(transform.position + transform.forward * range, radius);
    }


    public void specialItemNow(int id)
    {
        Debug.Log("special item!");

    }

    void attackDone() //called from our event when the animation is finished
    {
        isAttacking = false;
        //playerModel.rotation = lastRotation;
    }

    public void changeHeatlh(int amt){
        if(amt <= Health){Health += amt;}
        else{Health = 0;}
    }

    void gameOver(){
        isDead = true;
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "NPC":
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    yapper.startDialogue(other.gameObject.GetComponent<NPCInteraction>());
                }
                break;
            default:
                break;
        }
    }



}
