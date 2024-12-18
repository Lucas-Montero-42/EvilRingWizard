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

    private void Awake()
    {
        //Player con Transparent FX
        Physics.IgnoreLayerCollision(1,3, true);
        //Transparent FX con Transparent FX
        Physics.IgnoreLayerCollision(1,1, true);
        foreach (SpellParams s in spells)
        {
            s.canShoot = true;
        }

    }
    private void Start()
    {
        WeaponDiplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f && CanChangeSpell)
        {
            ChangeSpell(Input.GetAxisRaw("Mouse ScrollWheel"));
        }
    }
    public void ChangeSpell(float input)
    {
        //Play changing spell animation

        if (input > 0)
        {
            currentSpell--;
        }
        else
        {
            currentSpell++;
        }

        //Current Spell Edgecases
        if (currentSpell < 0)
        {
            currentSpell = spells.Count - 1;
        }
        else if (currentSpell > spells.Count - 1)
        {
            currentSpell = 0;
        }

        WeaponDiplay();
    }
    public void WeaponDiplay()
    {
        if (spells.Count == 0)
        {
            currentText.text = "";
            previousText.text = "";
            nextText.text = "";
            return;
        }

        currentText.text = spells[currentSpell].name;

        //Previous Spell Edgecase
        if (currentSpell - 1 < 0)
        {
            previousText.text = spells[spells.Count - 1].name;
        }
        else
        {
            previousText.text = spells[currentSpell - 1].name;
        }
        //Next Spell Edgecase
        if (currentSpell + 1 > spells.Count - 1)
        {
            nextText.text = spells[0].name;
        }
        else
        {
            nextText.text = spells[currentSpell + 1].name;
        }
    }
}
