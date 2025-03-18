using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f; // Destroy after time
    private float timer;


    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after set time
    }

    public void Launch(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }
    public void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Add logic for when the projectile hits something
        if (timer > .1f) {
            if (other.CompareTag("Enemy")) {
                if (gameObject.CompareTag("EnemyProjectile")) {
                    return; // Do not destroy or damage the enemy
                }
            }
            if (other.CompareTag("Player")) {
                if (gameObject.CompareTag("Projectile")) {
                    return;
                }
                other.GetComponent<Player>().health -= 20; // Reduce player's health
            }
            Destroy(gameObject);
            timer = 0;
        }
        
    }
}
