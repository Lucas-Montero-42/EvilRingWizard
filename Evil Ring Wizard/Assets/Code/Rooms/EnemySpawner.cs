using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    bool used = false;
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPositions;
    public GameObject effect;
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
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            float t = Random.Range(0, 1f);
            StartCoroutine(EnemyTimer(t, spawnPositions[i].transform)); // tiempo random entre 0 y 0.5f
        }
    }
    private IEnumerator EnemyTimer(float t, Transform position)
    {
        yield return new WaitForSeconds(t);
        Instantiate(effect, position.position, position.rotation);
        yield return new WaitForSeconds(2f);
        int e = Random.Range(0, enemyPrefabs.Length-1);
        Instantiate(enemyPrefabs[e], position.position, position.rotation);// Prefab random
        yield return null;
        // SpawnEnemies
            // Effecto de partículas (igual un simbolo arcano en el suelo)
            // Brillo y miniexplosión. 1.5s como máximo
            // Spawn enemigo en idle
            // Un poco de desfase entre cada enemigo que aparece. máximo de .5s entre el más rapido y el mas lento
    }
}
