using UnityEngine;

public class Player : MonoBehaviour
{
    //Setup
    public Camera mainCamera;
    public Transform playerHead;
    private CharacterController controller;
    //public Transform playerModel;

    //Movement
    public float moveSpeed = 10f;
    public float runSpeed;
    public float gravity = -9.81f;
    public bool isRunning = false;
    private Vector3 velocity;
    private Vector3 movementInput;


    //Animation and Attacking
    private Animator anim;
    public bool isMoving = false;
    public bool isAttacking = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        runSpeed = 2 * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //RotateToMouse();
        goMove();
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

    }


}
