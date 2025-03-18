using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    private float direction = 1;
    private float timer;
    public float moveTime = 2.5f;
    private Rigidbody2D rb;
    public static bool playerSpotted;
    public LayerMask player;
    public Animator animator;
    private bool idle;
    public Transform enemyFirePoint; // Assign in Inspector
    public GameObject projectilePrefab; // Assign in Inspector
    private bool isShooting = false; // Flag to check if shooting coroutine is running
    public float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 100;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (health <= 0) {
            Destroy(gameObject);
        }
        if (Player.paused || Player.playerDead) {
            animator.SetBool("playerSpotted", true);
            return;
        }

        Vector2 rayDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        playerSpotted = Physics2D.Raycast(transform.position, rayDirection, 7.5f, player);
        Debug.DrawRay(transform.position, rayDirection * 7.5f, Color.red);
        if (playerSpotted) {
            Debug.Log("Player Spotted");
            animator.SetBool("playerSpotted", true);
            idle = true;
            if (!isShooting) {
                StartCoroutine(Shoot());
            }
        }
        if (!playerSpotted) {
            Debug.Log("Player Not Spotted");
            animator.SetBool("playerSpotted", false);
            idle = false;
            isShooting = false; // Reset the flag when player is not spotted
            timer += Time.deltaTime;
            if (timer < moveTime) {
                rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            }
            if (timer >= moveTime) {
                timer = 0;
                direction *= -1f;
                Flip();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile")) {
            health -= 35f;
            if (other.CompareTag("EnemyProjectile")) {
                return;
            }
        }
    }

    private IEnumerator Shoot() {
        isShooting = true; // Set the flag to true when coroutine starts
        GameObject projectile = Instantiate(projectilePrefab, enemyFirePoint.position, Quaternion.identity);
        projectile.tag = "EnemyProjectile"; // Add this line to tag the projectile
        Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left; // Adjust for facing direction
        projectile.GetComponent<Projectile>().Launch(shootDirection);
        yield return new WaitForSeconds(1.5f);
        isShooting = false; // Reset the flag after waiting
    }

    private void Flip() {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
