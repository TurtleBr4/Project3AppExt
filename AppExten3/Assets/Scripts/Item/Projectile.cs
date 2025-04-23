using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private Rigidbody rb;
    public string shooterTag;


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
        if(shooterTag == collision.gameObject.tag)
        {
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().changeHealth(-damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().changeHeatlh(-damage);
        }
        else if (collision.gameObject.tag == "Helper")
        {
            collision.gameObject.GetComponent<Friendo>().takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
