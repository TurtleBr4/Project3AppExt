using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().changeHealth(damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().changeHeatlh(damage);
        }
        Destroy(gameObject);
    }
}
