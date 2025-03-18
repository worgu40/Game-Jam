using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;

public class ColorSwitch : MonoBehaviour
{
    private GameObject[] white;
    private GameObject[] black;
    public GameObject[] whiteUI;
    public GameObject[] blackUI;
    private bool canSwap;
    // True = white,  false = black
    [HideInInspector]
    public static bool swap;
    public float swapCooldown = 1;
    private float amount = 0;
    private AudioManager audioManager; // Add this line
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        amount = 0;
        white = GameObject.FindGameObjectsWithTag("White");
        black = GameObject.FindGameObjectsWithTag("Black");
        
        foreach (var obj in blackUI) {
            Debug.Log(obj.ToString() + "\n");
            obj.SetActive(false);
        }
        foreach (var i in whiteUI) {
            Debug.Log(i.ToString() + "\n");
        }
        canSwap = true;
        swap = false;
        audioManager = FindFirstObjectByType<AudioManager>(); // Add this line
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.paused) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canSwap) {
            StartCoroutine(Swap());
        }
    }
    private IEnumerator Swap() {
        Debug.Log(swap);
        white = GameObject.FindGameObjectsWithTag("White");
        black = GameObject.FindGameObjectsWithTag("Black");
        audioManager.PlaySwapSound(); // Add this line
        // Black to white
        if (swap) {
                Debug.Log("Black to white called");
                canSwap = false;
                Player.animator.SetTrigger("Black2White");
                Player.animator.SetBool("White", true);
            foreach (var i in white) {
                SpriteRenderer spriteRenderer;
                spriteRenderer = i.GetComponent<SpriteRenderer>();
                Color newColor = spriteRenderer.color;
                newColor.a = 255f;
                spriteRenderer.color = newColor;
                i.GetComponent<BoxCollider2D>().enabled = true;
                i.layer = LayerMask.NameToLayer("Ground");
                }
            foreach (var i in whiteUI) {
                i.SetActive(true);
            }
            foreach (var obj in black) {
                SpriteRenderer spriteRenderer;
                spriteRenderer = obj.GetComponent<SpriteRenderer>();
                Color newColor = spriteRenderer.color;
                newColor.a = 30f / 255f;
                spriteRenderer.color = newColor;
                obj.GetComponent<BoxCollider2D>().enabled = false;
                obj.layer = LayerMask.NameToLayer("Black");
                }
            foreach (var obj in blackUI) {
                obj.SetActive(false);
            }
        }
        // White to black
        if (!swap) {
                Debug.Log("White to black called");
                canSwap = false;
                Player.animator.SetTrigger("White2Black");
                Player.animator.SetBool("White", false);
                foreach (var obj in black) {
                    SpriteRenderer spriteRenderer;
                    spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    Color newColor = spriteRenderer.color;
                    newColor.a = 255f;
                    spriteRenderer.color = newColor;
                    obj.GetComponent<BoxCollider2D>().enabled = true;
                    obj.layer = LayerMask.NameToLayer("Ground");
                }
                foreach (var obj in blackUI) {
                    obj.SetActive(true);
                }
                foreach (var i in white) {
                    SpriteRenderer spriteRenderer;
                    spriteRenderer = i.GetComponent<SpriteRenderer>();
                    Color newColor = spriteRenderer.color;
                    newColor.a = 30f / 255f;
                    spriteRenderer.color = newColor;
                    i.GetComponent<BoxCollider2D>().enabled = false;
                    i.layer = LayerMask.NameToLayer("White");
                }
                foreach (var i in whiteUI) {
                    i.SetActive(false);
                }
             }
        swap = !swap;
        yield return new WaitForSeconds(swapCooldown);
        canSwap = true;
        }
    }
