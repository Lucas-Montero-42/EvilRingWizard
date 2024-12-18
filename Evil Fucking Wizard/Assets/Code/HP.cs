using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public int health;

    // ESTO ES DE DEBUG LUEGO HAY QUE CAMBIARLO POR OTRO METODO DE ELIMINAR ENEMIGOS -------------

    private void Awake()
    {

    }

    void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
    }
    IEnumerator Death()
    {
        //Desaparece con el tiempo
        // ALGUN MATERIAL DE DISOLVER DESPUES DE HACER UNA ANIMACIÓN -----------------------------
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
    //TAG DE VENENO
}
