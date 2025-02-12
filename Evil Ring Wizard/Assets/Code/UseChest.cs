using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseChest : MonoBehaviour
{
    public GameObject interactText;
    [SerializeField] bool interactEnabled = false;
    private bool open = false;
    public Animator animator;
    public AudioSource OpenSource;
    public AudioSource CloseSource;
    private GameObject ChestInventory;
    public GameObject InventoryPrefab;
    [SerializeField] private RingItem innateRing;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    void Start()
    {
        //interactText = CanvasSingleton.instance.InteractText;
        interactText.SetActive(false);
        InventoryPrefab.GetComponentInChildren<InventoryGridSystem>().innateRing = innateRing;
        ChestInventory = Instantiate(InventoryPrefab);
        ChestInventory.transform.SetParent(GameManager.instance.Canvas.transform);
        ChestInventory.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    void Update()
    {
        if (interactEnabled && Input.GetKey(KeyCode.E) )
        {
            //ABRIR EL MEN� DEL COFRE-------------------------------------------------------------------------
            GameManager.instance.HandsMenuScreen.SetActive(true);
            ChestInventory.SetActive(true);
            GameManager.instance.Pause();
            interactText.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Tab) && open && !GameManager.instance.holdingObject)// && GameManager.instance.state == GameManager.GameStates.Chest)
        {
            //CERRAR EL MEN� DEL COFRE------------------------------------------------------------------------
            ChestInventory.SetActive(false);
            GameManager.instance.HandsMenuScreen.SetActive(false);
            GameManager.instance.Resume();
            ChestAction(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        
        ChestAction(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;
        
        if (interactEnabled)
            ChestAction(false);
        
    }
    private void ChestAction(bool _open)
    {
        open = _open;
        interactText.SetActive(_open);
        interactEnabled = _open;
        animator.SetBool("Abierto", _open);

        if (_open) 
            OpenSource.Play();
        else 
            CloseSource.Play();
    }
}
