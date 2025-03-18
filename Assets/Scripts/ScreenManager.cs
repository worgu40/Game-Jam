using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this line

public class ScreenManager : MonoBehaviour
{
    public GameObject[] blackUI;
    public GameObject[] whiteUI;
    public GameObject[] otherUI;
    public GameObject[] worldTextCanvases; // Add this line
    public GameObject deathUI;
    public GameObject mainScreenUI;
    public GameObject menuUI;
    public GameObject SettingsUI;
    public GameObject mainScreenBackground;
    public GameObject covers;
    public bool hideUI;
    public bool bonusTips = true; // Add this line
    public static bool started = false;
    private bool cameFromMenuUI = false; // Add this line
    public Sprite hideUIImageOn; // Add this line
    public Sprite hideUIImageOff; // Add this line
    public Sprite bonusTipsImageOn; // Add this line
    public Sprite bonusTipsImageOff; // Add this line
    public Image hideUIToggleButton; // Add this line
    public Image bonusTipsToggleButton; // Add this line

    void Start()
    {
        started = false;
        bonusTips = true;
        foreach (GameObject obj in blackUI)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in whiteUI)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in otherUI)
        {
            obj.SetActive(false);
        }
        deathUI.SetActive(false);
        mainScreenUI.SetActive(true);
        menuUI.SetActive(false);
        SettingsUI.SetActive(false);
        mainScreenBackground.SetActive(true);
        covers.SetActive(true);
        UpdateHideUIButton(); // Add this line
        UpdateBonusTipsButton(); // Add this line
    }

    // Update is called once per frame
    void Update()
    {
        if (hideUI) {
            foreach (GameObject obj in blackUI)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in whiteUI)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in otherUI)
            {
                obj.SetActive(false);
            }
        }

        if (!bonusTips) {
            foreach (GameObject canvas in worldTextCanvases)
            {
                canvas.SetActive(false);
            }
        }
        if (bonusTips) {
            foreach (GameObject canvas in worldTextCanvases)
            {
                canvas.SetActive(true);
            }
        }
    }
    public void MainScreenSettings() {
        mainScreenUI.SetActive(false);
        SettingsUI.SetActive(true);
    }
    public void ExitButton() {
        SettingsUI.SetActive(false);
        if (cameFromMenuUI) { // Add this condition
            menuUI.SetActive(true);
        } else {
            mainScreenUI.SetActive(true);
            mainScreenBackground.SetActive(true);
            covers.SetActive(true);
        }
    }
    public void StartGame() {
        started = true;
        foreach (GameObject i in whiteUI) {
            i.SetActive(true);
        }
        foreach (GameObject i in otherUI) {
            i.SetActive(true);
        }
        mainScreenUI.SetActive(false);
        menuUI.SetActive(false);
        SettingsUI.SetActive(false);
        mainScreenBackground.SetActive(false);
        covers.SetActive(false);

    }

    public void OpenSettings()
    {
        cameFromMenuUI = menuUI.activeSelf; // Add this line
        menuUI.SetActive(false);
        SettingsUI.SetActive(true);
    }

    public void CloseMenu()
    {
        menuUI.SetActive(false);
        if (FindFirstObjectByType<Player>().health > 0) {
            foreach (var obj in blackUI) {
                obj.SetActive(true);
            }
            foreach (var i in whiteUI) {
                i.SetActive(true);
            }
            foreach (var other in otherUI) {
                other.SetActive(true);
            }
            Player.paused = false;
            Player.rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void ResumeGame()
    {
        menuUI.SetActive(false);
        if (FindFirstObjectByType<Player>().health > 0) {
            foreach (var obj in blackUI) {
                obj.SetActive(true);
            }
            foreach (var i in whiteUI) {
                i.SetActive(true);
            }
            foreach (var other in otherUI) {
                other.SetActive(true);
            }
            Player.paused = false;
            Player.rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void GoToTitleScreen()
    {
        mainScreenUI.SetActive(true);
        mainScreenBackground.SetActive(true);
        covers.SetActive(true);
        menuUI.SetActive(false);
        SettingsUI.SetActive(false);
        foreach (var obj in blackUI) {
            obj.SetActive(false);
        }
        foreach (var i in whiteUI) {
            i.SetActive(false);
        }
        foreach (var other in otherUI) {
            other.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void HiddenUI() {
        hideUI = !hideUI;
        UpdateHideUIButton(); // Add this line
    }
    public void BonusTips() {
        bonusTips = !bonusTips;
        UpdateBonusTipsButton(); // Add this line
    }

    private void UpdateHideUIButton() { // Add this method
        if (hideUI) {
            hideUIToggleButton.sprite = hideUIImageOn;
        } else {
            hideUIToggleButton.sprite = hideUIImageOff;
        }
    }

    private void UpdateBonusTipsButton() { // Add this method
        if (bonusTips) {
            bonusTipsToggleButton.sprite = bonusTipsImageOn;
        } else {
            bonusTipsToggleButton.sprite = bonusTipsImageOff;
        }
    }
}
