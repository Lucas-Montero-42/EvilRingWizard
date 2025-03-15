using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CreateNextLevelHere : MonoBehaviour
{
    GameObject interactText;
    bool interactable = false;
    public int maxLevel;
    private DungeonGenerator DG;
    // Start is called before the first frame update
    void Start()
    {
        interactText = GameManager.instance.InteractText;
        DG = GameObject.Find("DungeonGenerator").GetComponent<DungeonGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable && Input.GetKey(KeyCode.E))
        {
            StartCoroutine(LoadNewLevel());
        }
    }
    IEnumerator LoadNewLevel()
    {

        GameManager.instance.player.GetComponent<CharacterController>().enabled = false;
        GameManager.instance.player.transform.position = new Vector3(0, 0, 0);
        GameManager.instance.player.GetComponent<CharacterController>().enabled = true;
        if (DG.Level == maxLevel)
        {
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            DG.Level++;
            DG.RESETDUNGEON();
        }
        yield return new WaitForEndOfFrame();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        interactable = true;
        interactText.GetComponent<TextMeshProUGUI>().text = "Press E to Exit";
        interactText.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        interactable = false;
        interactText.SetActive(false);
    }
}
