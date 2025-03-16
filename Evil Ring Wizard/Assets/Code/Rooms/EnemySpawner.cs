using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    bool used = false;
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPositions;
    public GameObject effect;
    public int enemyNumber;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used)
        {
            SpawnEnemies();
            used = true;
        }        
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            StartCoroutine(EnemyTimer(i)); // tiempo random entre 0 y 0.5f
        }
    }
    private IEnumerator EnemyTimer(float t)
    {
        yield return new WaitForSeconds(t);
        Instantiate(effect);
        yield return new WaitForSeconds(1.5f);
        Instantiate(enemyPrefabs[0]);// Prefab random

        yield return null;
        // SpawnEnemies
            // Effecto de partículas (igual un simbolo arcano en el suelo)
            // Brillo y miniexplosión. 1.5s como máximo
            // Spawn enemigo en idle
            // Un poco de desfase entre cada enemigo que aparece. máximo de .5s entre el más rapido y el mas lento
    }
}
