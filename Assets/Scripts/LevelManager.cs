using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject level1;
    public static GameObject level2;
    public static GameObject level3;
    public GameObject level1End; // Add this line
    public GameObject level2End;
    public static Transform level3Start;
    public Transform level2StartPoint;

    // Add references to the background images and AudioManager
    public GameObject background1;
    public GameObject background2;
    public GameObject background3;
    public AudioManager audioManager;

    private void Start()
    {
        // Debug logs to check assignments
        Debug.Log("Background1: " + (background1 != null ? background1.name : "Not Assigned"));
        Debug.Log("Background2: " + (background2 != null ? background2.name : "Not Assigned"));
        Debug.Log("Background3: " + (background3 != null ? background3.name : "Not Assigned")); // Add this line
        Debug.Log("AudioManager: " + (audioManager != null ? audioManager.name : "Not Assigned"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Player.touchedLevel1End) {
                TransitionToLevel2(other.gameObject);
            }
            if (Player.touchedLevel2End) {
                TransitionToLevel3(other.gameObject, this);
            }
        }
    }

    private void TransitionToLevel2(GameObject player)
    {
        level2 = GameObject.Find("Level 2");
        if (level2 == null) {
            Debug.LogError("Level 2 not found!");
            return;
        }

        player.transform.position = level2StartPoint.position;
        level1.SetActive(false);
        level2.SetActive(true);

        // Ensure background1 and background2 are assigned
        if (background1 == null || background2 == null) {
            Debug.LogError("Backgrounds not assigned!");
            return;
        }

        // Ensure audioManager is assigned
        if (audioManager == null) {
            Debug.LogError("AudioManager not assigned!");
            return;
        }

        // Switch background and music
        background1.SetActive(false);
        background2.SetActive(true);
        audioManager.SwitchToCityMusic();
    }

    public static void TransitionToLevel3(GameObject player, LevelManager levelManager)
    {
        level2 = GameObject.Find("Level 2");
        level3 = GameObject.Find("Level 3");
        level3Start = GameObject.Find("Level 3 Start Point").transform;
        if (level2 == null || level3 == null || level3Start == null) {
            Debug.LogError("Level 3 transition objects not found!");
            return;
        }
        level2.SetActive(false);
        level3.SetActive(true);
        player.transform.position = level3Start.transform.position;

        // Ensure background2 and background3 are assigned
        if (levelManager.background2 == null || levelManager.background3 == null) {
            Debug.LogError("Backgrounds not assigned!");
            return;
        }

        // Ensure audioManager is assigned
        if (levelManager.audioManager == null) {
            Debug.LogError("AudioManager not assigned!");
            return;
        }

        // Switch background and music
        levelManager.background2.SetActive(false);
        levelManager.background3.SetActive(true);
        levelManager.audioManager.SwitchToForestMusic();
    }
}
