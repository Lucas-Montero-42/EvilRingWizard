using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    private List<SpellParams> previousSpells = new List<SpellParams>(); // Lista de hechizos anteriores
    private SpellShooter spellShooter;

    // Compara con la ultima mano vista para saber que hechizos añadir
    private Grid<Item> previousHand;

    private void Awake()
    {
        spellShooter = GetComponent<SpellShooter>();
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
        handsInventory.pickUpItem += RemoveSpell;

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
        WeaponDiplay();
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
        //WeaponDiplay();
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
        if (spells[currentSpell].canShoot)
            currentText.color = Color.green;
        else
            currentText.color = Color.red;


        // Previous Spell Edgecase
        if (currentSpell - 1 < 0)
        {
            previousText.text = spells[spells.Count - 1].name;
            if (spells[spells.Count - 1].canShoot)
                previousText.color = Color.green;
            else
                previousText.color = Color.red;
        }
        else
        {
            previousText.text = spells[currentSpell - 1].name;
            if (spells[currentSpell - 1].canShoot)
                previousText.color = Color.green;
            else
                previousText.color = Color.red;
        }
        // Next Spell Edgecase
        if (currentSpell + 1 > spells.Count - 1)
        {
            nextText.text = spells[0].name;
            if (spells[0].canShoot)
                nextText.color = Color.green;
            else
                nextText.color = Color.red;
        }
        else
        {
            nextText.text = spells[currentSpell + 1].name;
            if (spells[currentSpell + 1].canShoot)
                nextText.color = Color.green;
            else
                nextText.color = Color.red;
        }
        //Cambio de color según disponibilidad
        
        
    }
    public static T CloneScriptableObject<T>(T original) where T : ScriptableObject
    {
        T instance = Instantiate(original);
        instance.name = original.name;
        return instance;
    }
    private void AddSpell()
    {
        // Obtiene el anillo actualmente colocado en la mano
        RingItem ring = GameManager.instance.lastRingItem;

        // Verifica si hay un anillo y si tiene un hechizo asociado
        if (ring != null && ring.spell != null)
        {
            SpellParams clonedSpell = CloneScriptableObject(ring.spell);
            spells.Add(clonedSpell);
            WeaponDiplay(); // Actualiza la UI de hechizos
        }
    }

    public void RemoveSpell()
    {
        SpellParams spellToRemove = handsInventory.GetCurrentItem().spell;

        SpellParams foundSpell = spells.FirstOrDefault(s => s.name == spellToRemove.name);
        if (foundSpell != null)
        {
            spells.Remove(foundSpell);
            WeaponDiplay();
        }
    }
}
