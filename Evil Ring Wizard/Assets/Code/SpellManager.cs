using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public List<SpellParams> spells;
    public int currentSpell = 0;
    private bool CanChangeSpell = true;
    public float SChangeCooldown = 0.5f;

    public TextMeshProUGUI currentText;
    public TextMeshProUGUI previousText;
    public TextMeshProUGUI nextText;
    public InventoryGridSystem handsInventory;

    // Compara con la ultima mano vista para saber que hechizos añadir
    private Grid<Item> previousHand;

    private void Awake()
    {
        // Evita colision de Player con Transparent FX
        Physics.IgnoreLayerCollision(1,3, true);

        // Evita colision de Transparent FX con Transparent FX
        Physics.IgnoreLayerCollision(1,1, true);

        // Resetea el cooldown de todos los hechizos
        foreach (SpellParams s in spells)
        {
            s.canShoot = true;
        }

        handsInventory.dropItem += AddSpell;

    }
    private void Start()
    {
        WeaponDiplay();
    }

    void Update()
    {
        // Cambia de hechizo con la rueda del ratón
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f && CanChangeSpell)
        {
            ChangeSpell(Input.GetAxisRaw("Mouse ScrollWheel"));
        }
    }
    public void ChangeSpell(float input)
    {
        //PLAY CHANGING SPELL ANIMATION (FALTA)------------------------------------------------------------

        // Pasa al siguiente en función de la rueda del ratón
        if (input > 0)
        {
            currentSpell--;
        }
        else
        {
            currentSpell++;
        }

        // Permite dar la vuelta a la lista (Current Spell Edgecases)
        if (currentSpell < 0)
        {
            currentSpell = spells.Count - 1;
        }
        else if (currentSpell > spells.Count - 1)
        {
            currentSpell = 0;
        }

        // Muestra el hechizo que esás usando y los inmediatamente anterior y posterior
        WeaponDiplay();
    }
    public void WeaponDiplay()
    {
        // Si no hay hechizos no muestres nada
        if (spells.Count == 0)
        {
            currentText.text = "";
            previousText.text = "";
            nextText.text = "";
            return;
        }

        currentText.text = spells[currentSpell].name;

        // Previous Spell Edgecase
        if (currentSpell - 1 < 0)
        {
            previousText.text = spells[spells.Count - 1].name;
        }
        else
        {
            previousText.text = spells[currentSpell - 1].name;
        }
        // Next Spell Edgecase
        if (currentSpell + 1 > spells.Count - 1)
        {
            nextText.text = spells[0].name;
        }
        else
        {
            nextText.text = spells[currentSpell + 1].name;
        }
    }
    private void AddSpell()
    {
        Grid<Item> newHand = handsInventory.GetHand();
        if (previousHand == null)
        {
            for (int x = 0; x < handsInventory.width; x++)
            {
                for (int y = 0; y < handsInventory.height; y++)
                {
                    if (newHand.GetGridObject(x, y).GetPlacedItem() != null)
                    {
                        spells.Add(newHand.GetGridObject(x, y).GetPlacedItem().GetRingItem().spell);
                        WeaponDiplay();
                        Debug.Log("Primera");
                        previousHand = newHand;
                        return;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Segunda");
            for (int x = 0; x < handsInventory.width; x++)
            {
                for (int y = 0; y < handsInventory.height; y++)
                {
                    if (newHand.GetGridObject(x, y).GetPlacedItem() != null)
                    {
                        Debug.Log(newHand.GetGridObject(x, y).GetPlacedItem().GetRingItem().spell);
                        Debug.Log(previousHand.GetGridObject(x, y).GetPlacedItem().GetRingItem().spell);
                    }
                    //Por algún motivo, la mano nueva y la vieja son iguales, es decir que la mano vieja se actualiza en alguna parte y yo no lo entiendo
                    if (newHand.GetGridObject(x, y).GetPlacedItem() != null && newHand.GetGridObject(x, y).GetPlacedItem().GetRingItem().spell != previousHand.GetGridObject(x, y).GetPlacedItem().GetRingItem().spell)
                    {
                        spells.Add(newHand.GetGridObject(x, y).GetPlacedItem().GetRingItem().spell);
                        WeaponDiplay();
                        Debug.Log("GOOD");
                        previousHand = newHand;
                        return;
                    }
                }
            }
            Debug.Log("Cagaste");
        }
    }
}
