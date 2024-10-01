using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    private enum GameState { Play, Pause};
    [SerializeField] private GameState state;

    // Objects to turn off/on
    public GameObject Player;
    public Camera MainCam;
    public GameObject EnemySpawnManger;

    public GameObject MainMenu;
    public Camera DefaultCam;

    public GameObject Crosshair;
    public GameObject Stats;

    void Start()
    {
        state = GameState.Pause;
        ToggleObjects(false);
        UnlockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (state == GameState.Play)
            {
                state = GameState.Pause;
                MainMenu.SetActive(true);
                Time.timeScale = 0;
                UnlockCursor();
            }
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
        Cursor.visible = false; // Hides the cursor
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true; // Shows the cursor
    }

    void ToggleObjects(bool Flag)
    {
        // True flag things
        Player.SetActive(Flag);
        EnemySpawnManger.SetActive(Flag);
        MainCam.gameObject.SetActive(Flag);
        Crosshair.gameObject.SetActive(Flag);
        Stats.gameObject.SetActive(Flag);

        // Inverse True Flag things
        MainMenu.gameObject.SetActive(!Flag);
        DefaultCam.gameObject.SetActive(!Flag);

    }

    public void Play()
    {
        Debug.Log("Play is pressed");

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            MainMenu.SetActive(false);
        }
        else
        {
            ToggleObjects(true);
            
        }
        LockCursor();
        state = GameState.Play;
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
