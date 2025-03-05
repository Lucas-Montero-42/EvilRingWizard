using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UseNextLevelDoor : MonoBehaviour
{
    GameObject interactText;
    bool interactable = false;
    public string level;
    // Start is called before the first frame update
    void Start()
    {
        interactText = GameManager.instance.InteractText;
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
        GameManager.instance.player.transform.position = new Vector3(0, 0, 0);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(level);
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
