using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // PART OF DATA
    public float health = 100;
    public int currentLevel = 1;
    [HideInInspector]
    public bool isFacingRight = true;
    [HideInInspector]
    public bool? rightJump = null;
    public float levelTimer;
    public float minutes;

    


    // OTHER VARIABLES
    // MOVEMENT
    private float horizontal;
    public static Rigidbody2D rb;
    public float speed;
    private bool canJump;
    public float jumpCooldown;
    public float jumpPower;
    private bool isGrounded;
    public float gravityMultiplier;
    private bool wasGrounded;
    public TMP_Text collectText;
    public TMP_Text timeText;
    public static bool paused;
    private bool resume;
    private int roundedTime;
    [HideInInspector]
    public float collectAmount;
    public GameObject[] blackUI;
    public GameObject[] whiteUI;
    public GameObject[] otherUI;
    public GameObject deathUI;
    public GameObject menuUI;
    // DASH
    public TrailRenderer tr;
    private bool canDash;
    private bool isDashing;
    public float dashingPower;
    private float dashingTime = 0.25f;
    public float dashCooldown;
    // WALL
    private bool isWallSliding;
    public float wallSlideSpeed;
    public float wallJumpSpeed;
    private bool isWallJumping;
    // MISC
    public LayerMask groundLayer;
    [HideInInspector]
    public static Animator animator;
    [HideInInspector]
    public float currentSpeed;
    public static bool playerDead;
    public Vector2 lastCheckpointPosition; // Add this variable to store the last checkpoint position
    public Image blackHealthBar; // Add this variable for black health bar
    public Image whiteHealthBar; // Add this variable for white health bar
    public static bool touchedLevel1End;
    public static bool touchedLevel2End;
    public static bool touchedLevel3End;
    public GameObject levelManager;
    private ScreenManager screenManager;
    private AudioManager audioManager; // Add this line

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            if (paused) {
                foreach (var obj in blackUI) {
                    obj.SetActive(false);
                    Debug.Log("Paused: Setting blackUI to inactive");
                }
                foreach (var i in whiteUI) {
                    i.SetActive(false);
                }
                foreach (var other in otherUI) {
                    other.SetActive(false);
                }
                menuUI.SetActive(true);
                rb.bodyType = RigidbodyType2D.Static;
            }
            if (!paused && health > 0) { // Change condition to ensure health is greater than 0
                foreach (var obj in blackUI) {
                    obj.SetActive(true);
                    Debug.Log("Unpaused: Setting blackUI to active");
                }
                foreach (var i in whiteUI) {
                    i.SetActive(true);
                }
                foreach (var other in otherUI) {
                    other.SetActive(true);
                }
                menuUI.SetActive(false);
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

        }
        // WHEN PLAYER DIES
        if (health <= 0) {
            blackHealthBar.fillAmount = 0;
            whiteHealthBar.fillAmount = 0;
            playerDead = true;
            Debug.Log("Player died: Setting blackUI to inactive");
            foreach (var obj in blackUI) {
                obj.SetActive(false);
            }
            foreach (var i in whiteUI) {
                i.SetActive(false);
            }
            foreach (var other in otherUI) {
                other.SetActive(false);
            }
            deathUI.SetActive(true);
            rb.bodyType = RigidbodyType2D.Static;
        }
        if (paused || health <= 0) {
            return;
        }
        if (!paused)
        if (ScreenManager.started) {
        levelTimer += Time.deltaTime;
        roundedTime = Mathf.FloorToInt(levelTimer);
        if (roundedTime == 60) {
            roundedTime = 0;
            levelTimer = 0;
            minutes += 1;
        }
        // 1:07
        if (minutes > 10) {
            timeText.fontSize = 54;
        }
        if (minutes < 10) {
            timeText.fontSize = 72;
        }
        if (roundedTime < 10) {
            timeText.text = minutes.ToString() + ":0" + roundedTime.ToString();
        }
        if (roundedTime >= 10) {
            timeText.text = minutes.ToString() + ":" + roundedTime.ToString();
        }
        // Update health bars
        blackHealthBar.fillAmount = health / 100f;
        whiteHealthBar.fillAmount = health / 100f;
        }
        // Locks movement if the player is dashing
        if (isDashing) {
            return;
        }
        // Grabs the direction of the player (-1 equals left, 1 equals right, 0 equals no direction)
        horizontal = Input.GetAxisRaw("Horizontal");
        
        // Jumping feature
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded) {
            if (animator.GetBool("White")) {
            animator.Play("Player_Jump");
            }
            if (!animator.GetBool("White")) {
                animator.Play("Player_JumpB");
            }
            canJump = true;
            StartCoroutine(Jump());
            audioManager.PlayJumpSound(); // Add this line
        }
        // Fast Falling
        if (Input.GetKey(KeyCode.S) && !isGrounded) {
            FastFall();
        }
        // Dashing
        if (Input.GetKeyDown(KeyCode.E) && canDash) {
            StartCoroutine(Dash());
        }
        // Wall Jump
        if (isGrounded) {
            rightJump = null;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))&& !isGrounded && isOnWall()) {
            WallJump();
        }
        
        // Check if grounded 
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        // 
        if (!isGrounded && wasGrounded) {
            Debug.Log("Player is in air");
            wasGrounded = false;
        }
        if (isGrounded && !wasGrounded) {
            Debug.Log("Player just landed");
            if (animator.GetBool("White")) {
            animator.Play("Player_Land");
            }
            if (!animator.GetBool("White")) {
                animator.Play("Player_LandB");
            }
            wasGrounded = true;
            audioManager.PlayLandSound(); // Add this line
        }
        // Test features

        // DEBUGS

        // Grounded Check Debug
        Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.red);
        // Wall Check Debug
        if (isFacingRight) {
            Debug.DrawRay(transform.position, Vector2.right * 0.6f, Color.blue);
        }
        if (!isFacingRight) {
            Debug.DrawRay(transform.position, Vector2.left * 0.6f, Color.blue);
        }

        // Animations
        animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
        
        // Methods to call
        Flip();
        WallSlide();
        isOnWall();
        CalculateSpeed();
        if (Mathf.Abs(horizontal) > 0.1f && isGrounded)
        {
            audioManager.PlayMovingSound(); // Add this line
        }
        else
        {
            audioManager.StopMovingSound(); // Add this line
        }
    }
    private void FixedUpdate()
    {
        if (paused) {
            return;
        }
        // Locks movement if dashing
        if (isDashing) {
            return;
        }

        Move();
        
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("White", true);
        rb = GetComponent<Rigidbody2D>();
        rightJump = null;
        canDash = true;
        LoadGame(); // Load player data when the game starts (need to change to a button)
        if (isFacingRight) {
            transform.localScale = new Vector2(1, 1);
        }
        if (!isFacingRight) {
            transform.localScale = new Vector2(-1, 1);
        }
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        wasGrounded = isGrounded;
        screenManager = FindFirstObjectByType<ScreenManager>();
        audioManager = FindFirstObjectByType<AudioManager>(); // Add this line
    }

    public void SaveGame()
    // Whenever called, used to save game data to the file.
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadGame()
    // TO DO: if theres no data, make a new save game
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            transform.position = new Vector2(data.positionX, data.positionY);
            health = data.health;
            currentLevel = data.currentLevel;
            isFacingRight = data.isFacingRight;
            rightJump = data.rightJump;
            levelTimer = data.levelTimer;
            minutes = data.minutes;
        }
    }
    private void Move() {
        rb.linearVelocity = new Vector2(speed * horizontal, rb.linearVelocityY);
    }
    private IEnumerator Jump() {
        canJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
    private void FastFall() {
        rb.linearVelocity += Vector2.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
    }
    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (isFacingRight) {
        rb.linearVelocity = new Vector2(dashingPower, 0f);
        }
        if (!isFacingRight) {
        rb.linearVelocity = new Vector2(-dashingPower, 0f);
        }
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private bool isOnWall() {
        if (isFacingRight) {
            return Physics2D.Raycast(transform.position, Vector2.right, 0.6f, groundLayer);
        }
        if (!isFacingRight) {
            return Physics2D.Raycast(transform.position, Vector2.left, 0.6f, groundLayer);
        }
        else {
        return false;
        }
    }
    private void WallSlide() {
        if (isOnWall() && !isGrounded) {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else {
            isWallSliding = false;
        }
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f || isWallJumping == true)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void WallJump() {
        if (isFacingRight) {
            if (rightJump == true || rightJump == null) {
                isWallJumping = true;
                Flip();
                rb.linearVelocity = new Vector2(8, wallJumpSpeed);
                rightJump = false;
                isWallJumping = false;
                return;
            }
        }
        if (!isFacingRight) {
            if (rightJump == false || rightJump == null) {
                isWallJumping = true;
                Flip();
                rb.linearVelocity = new Vector2(-8, wallJumpSpeed);
                rightJump = true;
                isWallJumping = false;
                return;
            }
        }
    }
    private float CalculateSpeed() {
    
        if (isGrounded) {
        currentSpeed = horizontal * speed;
        return currentSpeed;
        }
        if (!isGrounded) {
            currentSpeed = 0;
            return currentSpeed;
        }
        else {
            return currentSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Level1End")) {
            touchedLevel1End = true;
        }
        if (collision.CompareTag("Level2End")) {
            touchedLevel2End = true;
            LevelManager.TransitionToLevel3(gameObject, levelManager.GetComponent<LevelManager>());
        }
        if (collision.CompareTag("Level3End")) {
            LevelManager.TransitionToLevel3(gameObject, levelManager.GetComponent<LevelManager>());
        }
        if (collision.CompareTag("Collectibles")) {
            collectAmount += 1;
            collectText.text = collectAmount.ToString();
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag("Checkpoint")) {
            Debug.Log("Making Checkpoint");
            lastCheckpointPosition = collision.transform.position; // Set the last checkpoint position
        }
    }
    // Any testing methods go here.
    public void IncreaseHealth() {
        health += 100;
    }
    public void OnApplicationQuit()
    {
        // SaveGame();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Spikes")) {
            health = 0;

        }
    }
    public void ResetPlayer() {
        Debug.Log("Resetting Player");
        health = 100;
        playerDead = false;
        foreach (var obj in blackUI) {
            obj.SetActive(true);
        }
        foreach (var i in whiteUI) {
            i.SetActive(true);
        }
        foreach (var other in otherUI) {
            other.SetActive(true);
        }
        deathUI.SetActive(false); // Set deathUI to inactive
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.position = lastCheckpointPosition; // Move player to the last checkpoint position
    }
}

