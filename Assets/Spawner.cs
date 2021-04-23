using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> projectiles;
    public float spawnRate;
    private float spawnedTime;
    public float spawnRadius;
    public float radiusMinMult;
    public float radiusMaxMult;
    public float spawnRateDelta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnedTime + spawnRate)
        {
            Instantiate(projectiles[Random.Range(0, 9)], 
                            Random.insideUnitCircle.normalized 
                            * spawnRadius 
                            * Random.Range(radiusMinMult, radiusMaxMult),
                            Quaternion.identity);
            spawnedTime = Time.time;
            spawnRate += spawnRateDelta;
            if (spawnRate < 0.3f) spawnRate = 0.3f;
        }
    }
}
