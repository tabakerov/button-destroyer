using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public Alphabet alphabet;
    public float spawnRate;
    private float spawnedTime;
    public float spawnRadius;
    public float radiusMinMult;
    public float radiusMaxMult;
    public float spawnRateDelta;

    public AudioClip spawnSound;

    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnedTime + spawnRate)
        {
            sound.PlayOneShot(spawnSound);
            var newborn = Instantiate(alphabet.letters[Random.Range(0, alphabet.letters.Count)], 
                            Random.insideUnitCircle.normalized 
                            * spawnRadius 
                            * Random.Range(radiusMinMult, radiusMaxMult),
                            Quaternion.identity);
            spawnedTime = Time.time;
            spawnRate += spawnRateDelta;
            var forcePosition = newborn.transform.position;
            forcePosition.x += Random.Range(-1f, 1f);
            forcePosition.y += Random.Range(-1f, 1f);
            newborn.GetComponent<Rigidbody2D>().AddTorque(
                            Random.Range(-5f, 5f));
            if (spawnRate < 0.3f) spawnRate = 0.3f;
        }
    }
}
