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
            if (state == GameStates.Pause)
            {
                Resume();
            }
            else if (state == GameStates.Play)
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        //AÑADIR MENU DE PAUSA
        //PauseMenuScreen.SetActive(true);
        MouseLock(false);
        PlayerLock(true);
        Time.timeScale = 0;
        StartCoroutine(ChangeState(GameStates.Pause));
    }
    public void Resume()
    {
        //PauseMenuScreen.SetActive(false);
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
