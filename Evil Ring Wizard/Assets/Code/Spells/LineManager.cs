using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class LineManager : MonoBehaviour
{
    public GameObject[] particles;
    public float range = 30f;
    public float spacing = 1.5f;
    public float timeSpacing = .25f;
    public float particleSpeed = 2f;
    public float particleUpTime = 1f;
    private List<Transform> goingUp = new List<Transform>();
    private List<Transform> goingDown = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RemoveLine());
        //Spawnear partículas aleatorias en toda la linea
        for (int i = 0; i < range/spacing; i++)
        {
            int rand;
            rand = Random.Range(0,particles.Length);
            GameObject particle = Instantiate(particles[rand],transform);
            particle.transform.position = transform.position + transform.forward * i * spacing + Vector3.down * particle.transform.localScale.y/2;
            StartCoroutine(MoveParticle(particle.transform,i));
        }
    }
    // Update is called once per frame
    void Update()
    {
        List<Transform> goingUpCopy = new List<Transform>(goingUp);
        List<Transform> goingDownCopy = new List<Transform>(goingDown);

        foreach (Transform t in goingUpCopy)
        {
            GoingUP(t);
        }
        foreach (Transform t in goingDownCopy)
        {
            GoingDown(t);
        }
    }
    IEnumerator MoveParticle(Transform p, int i)
    {
        yield return new WaitForSeconds(timeSpacing * i);
        goingUp.Add(p);
        yield return new WaitForSeconds(particleUpTime);
        goingUp.Remove(p);
        goingDown.Add(p);
    }
    private void GoingUP(Transform t)
    {
        if(t.position.y < t.localScale.y/2)
            t.position += Vector3.up * particleSpeed * Time.deltaTime;
    }
    private void GoingDown(Transform t)
    {
        if (t.position.y > -t.localScale.y)
            t.position += Vector3.down * particleSpeed * Time.deltaTime;
    }
    IEnumerator RemoveLine()
    {
        yield return new WaitForSeconds(range / spacing * timeSpacing + particleUpTime + .5f);
        Destroy(gameObject);
    }
}
