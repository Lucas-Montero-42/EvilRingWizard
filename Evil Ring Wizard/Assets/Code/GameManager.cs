using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameStates
    {
        Pause,
        Play,
        MainMenu
    }
    public GameStates state;
    public GameObject player;
    public GameObject PauseMenuScreen;
    public GameObject HandsMenuScreen;
    public GameObject Canvas;
    public float mouseSensitivity = 0.75f;// AÑADIR OPCIONES EN EL MENÚ PARA CAMBIARLO--------------------------------------
    public bool HoldingObject = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (state == GameStates.Pause && !HoldingObject)
            {
                Resume();
                PauseMenuScreen.SetActive(false);
            }
            else if (state == GameStates.Play)
            {
                Pause();
                PauseMenuScreen.SetActive(true);
            }
        }
    }
    public void Pause()
    {
        MouseLock(false);
        PlayerLock(true);
        Time.timeScale = 0;
        StartCoroutine(ChangeState(GameStates.Pause));
    }
    public void Resume()
    {
        MouseLock(true);
        PlayerLock(false);
        Time.timeScale = 1;
        StartCoroutine(ChangeState(GameStates.Play));
    }
    private void MouseLock(bool Locked)
    {
        if (Locked)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void PlayerLock(bool Locked)
    {
        player.GetComponent<PlayerMovement>().enabled = !Locked;
        player.GetComponent<MouseLook>().enabled = !Locked;
        player.GetComponent<SpellManager>().enabled = !Locked;
        player.GetComponent<SpellShooter>().enabled = !Locked;
    }
    IEnumerator ChangeState(GameStates _state)
    {
        yield return new WaitForEndOfFrame();
        state = _state;
    }
}
