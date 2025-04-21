using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
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
