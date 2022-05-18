using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetorSpawner : MonoBehaviour
{
    
    public float spawnWaitTime = 1f;
    public GameObject metor;

    private float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time;
    }

    private void Update()
    {
        float currentTime = Time.time;
        if (currentTime >= nextSpawnTime)
        {
            SpawnMetor();
            nextSpawnTime = currentTime + spawnWaitTime;
        }
    }

    private void SpawnMetor()
    {
        GameObject newMetor = (GameObject)Instantiate(metor, transform);

        newMetor.transform.position = new Vector3(0, 0, GameManager.Instance.spaceDepth);
        newMetor.GetComponent<Renderer>().material.color = new Color(Random.Range(0,1f), Random.Range(0, 1f), Random.Range(0, 1f));

        Vector3 rotation = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        newMetor.transform.rotation = Quaternion.Euler(rotation);
    }

}
