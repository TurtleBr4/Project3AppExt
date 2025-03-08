using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    //variables and such 
    //first for moving
    public float speed;
    private float xmove;
    private float zmove;

    //now for grabbing rigidbodies
    private Rigidbody prb;

    private void Start()
    {
        prb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        xmove = Input.GetAxis("Horizontal");
        zmove = Input.GetAxis("Vertical");

        //calc the position
        Vector3 movement = new Vector3(xmove, 0, zmove) * speed * Time.deltaTime;

        //move to the new position
        transform.Translate(movement);
    }
}
