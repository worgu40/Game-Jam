using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject whiteProjectilePrefab; // Assign in Inspector
    public GameObject blackProjectilePrefab; // Assign in Inspector
    public Transform firePoint; // Assign in Inspector
    public Transform enemyFirePoint; // Assign in Inspector
    private bool canShoot = true; // Flag to control shooting

    void Start()
    {
        enemyFirePoint = GameObject.FindGameObjectsWithTag("EnemyFirePoint")[0].transform;
        Debug.Log("ENEMY FIRE POINT IS: " + enemyFirePoint);
    }

    void Update()
    {
        if (Player.paused || Player.playerDead) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F)) // Change key if needed
        {
            if (canShoot) {
            StartCoroutine(Shoot());
            }
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        GameObject projectilePrefab = ColorSwitch.swap ? blackProjectilePrefab : whiteProjectilePrefab;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left; // Adjust for facing direction
        projectile.GetComponent<Projectile>().Launch(shootDirection);
        yield return new WaitForSeconds(.5f);
        canShoot = true;
    }
}
