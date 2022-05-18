using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipSmokeEffect : MonoBehaviour
{
    new ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        var m = particleSystem.main; 
        m.startSize3D = true;
    }

    // Update is called once per frame
    void Update()
    {
        var main = particleSystem.main;

        float size = Random.Range(1f, 5f);
        main.startSizeX = size;
        main.startSizeY = size;
        main.startSizeZ = size;
    }
}
